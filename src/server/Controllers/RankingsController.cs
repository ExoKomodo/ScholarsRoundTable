using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using server.Mappers;
using server.Models;
using server.Repositories;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RankingsController : ControllerBase
    {
        public class CommentaryRank
        {
            public int CommentaryId { get; set; }
            public string CommentaryName { get; set; }
            public string CommentaryIsbn { get; set; }
            public float Rank { get; set; }
        }

        private readonly ILogger<RankingsController> _logger;
        private readonly IMapper<Ranking> _rankingMapper;
        private readonly IRepository<Commentary> _commentaryRepository;
        private readonly IRepository<Ranking> _rankingRepository;
        private static readonly string CalculateRankingsQuery = @"
            SELECT x.authority_id, x.commentary_id, x.book_id, x.ranking, x.seminary_id
            FROM (
                SELECT r.authority_id, r.book_id, r.commentary_id, r.ranking, a.seminary_id
                FROM (
                    SELECT authority_id, book_id, commentary_id, ranking
                    FROM Rankings
                ) AS r
                INNER JOIN (
                    SELECT id, seminary_id
                    FROM Authorities
                    WHERE seminary_id = {1}
                ) AS a ON r.authority_id = a.id
            ) AS x
            INNER JOIN (
                SELECT commentary_id, book_id
                FROM CommentaryBook
                WHERE book_id = {0}
            ) AS y ON x.book_id = y.book_id AND x.commentary_id = y.commentary_id
            ORDER BY x.commentary_id
        ";

        public RankingsController(ILogger<RankingsController> logger, IMapper<Ranking> rankingMapper, IRepository<Commentary> commentaryRepository, IRepository<Ranking> rankingRepository)
        {
            _logger = logger;
            _rankingMapper = rankingMapper;
            _commentaryRepository = commentaryRepository;
            _rankingRepository = rankingRepository;
        }

        [HttpPost]
        public ActionResult<Ranking> Create([FromBody] Ranking ranking)
        {
            if (_rankingRepository.Create(ranking) == 0)
            {
                return BadRequest();
            }
            else
            {
                return Ok(ranking);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommentaryRank>> CalculateRankings([FromQuery] int bookId, [FromQuery] int seminaryId)
        {
            try
            {
                var rankings = new List<Ranking>();
                var ranks = new Dictionary<int, float>();
                using (var connection = new MySqlConnection(Startup.ConnectionString))
                {
                    connection.Open();

                    string query = String.Format(CalculateRankingsQuery, bookId, seminaryId);
                    var command = new MySqlCommand(query, connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new List<object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader[i]);
                            }
                            rankings.Add(_rankingMapper.FromFieldList(row));
                        }
                    }
                }

                var commentaryRankings = new Dictionary<int, float>();
                var numberOfCommentaryRankers = new Dictionary<int, int>();
                var authorityRankings = new Dictionary<int, float>();
                foreach (var ranking in rankings)
                {
                    var commentaryId = ranking.CommentaryId;
                    if (!commentaryRankings.ContainsKey(commentaryId))
                    {
                        commentaryRankings[commentaryId] = 0;
                    }
                    commentaryRankings[commentaryId] += ranking.Rank;
                    if (!numberOfCommentaryRankers.ContainsKey(commentaryId))
                    {
                        numberOfCommentaryRankers[commentaryId] = 0;
                    }
                    numberOfCommentaryRankers[commentaryId] += 1;
                    authorityRankings[ranking.AuthorityId] = 1;
                }
                float totalRankers = (float)authorityRankings.Count;
                foreach (var item in Enumerable.Zip(commentaryRankings, numberOfCommentaryRankers))
                {
                    var commentaryRanking = item.First;
                    var countOfCommentaryRanker = item.Second;

                    ranks[commentaryRanking.Key] = (
                        (
                            commentaryRanking.Value
                            / totalRankers
                        )
                        * (
                            countOfCommentaryRanker.Value
                            / totalRankers
                        )
                    );
                }

                var commentaries = _commentaryRepository.GetAll();
                var commentaryRanks = new List<CommentaryRank>();
                foreach (var commentary in commentaries)
                {
                    if (ranks.ContainsKey(commentary.Id))
                    {
                        commentaryRanks.Add(new CommentaryRank
                        {
                            CommentaryId = commentary.Id,
                            CommentaryName = commentary.Name,
                            CommentaryIsbn = commentary.Isbn,
                            Rank = ranks[commentary.Id],
                        });
                    }
                }

                return Ok(commentaryRanks);
            }
            catch (Exception ex)
            {
                var message = $"Rankings Controller (CalculateRankings): {ex.Message}";
                Console.WriteLine(message);
                return BadRequest(message);
            }
        }

        [HttpGet("query")]
        public ActionResult<IEnumerable<Ranking>> Query([FromQuery] int authorityId, [FromQuery] int bookId, [FromQuery] int commentaryId)
        {
            var result = new List<Ranking>();
            var queryFields = new Dictionary<string, int>();
            if (authorityId > 0)
            {
                queryFields["authority_id"] = authorityId;
            }
            if (bookId > 0)
            {
                queryFields["book_id"] = bookId;
            }
            if (commentaryId > 0)
            {
                queryFields["commentary_id"] = commentaryId;
            }
            var query = "SELECT * FROM Rankings";
            if (queryFields.Count > 0)
            {
                query += " WHERE ";
                var whereClause = new List<string>();
                foreach (var field in queryFields)
                {
                    whereClause.Add($"{field.Key} = {field.Value}");
                }
                query += String.Join(" AND ", whereClause);
            }

            try
            {
                using (var connection = new MySqlConnection(Startup.ConnectionString))
                {
                    connection.Open();

                    var command = new MySqlCommand(query, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new List<object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row.Add(reader[i]);
                            }
                            result.Add(_rankingMapper.FromFieldList(row));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Repository Helper (GetById): {ex.Message}");
            }
            return Ok(result);
        }

        [HttpPut]
        public ActionResult<Ranking> Update([FromBody] Ranking ranking)
        {
            var query = $"UPDATE Rankings SET ranking = {ranking.Rank} WHERE authority_id = {ranking.AuthorityId} AND book_id = {ranking.BookId} AND commentary_id = {ranking.CommentaryId}";

            using (var connection = new MySqlConnection(Startup.ConnectionString))
            {
                connection.Open();

                var command = new MySqlCommand(query, connection);
                if (command.ExecuteNonQuery() == 0)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(ranking);
                }
            }
        }
    }
}
