using System;
using System.Collections.Generic;
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
    public class CommentariesController : ControllerBase
    {
        public class CommentaryCreateModel
        {
            public IEnumerable<int> BookIds { get; set; }
            public string Isbn { get; set; }
            public string Name { get; set; }
        }

        private readonly ILogger<CommentariesController> _logger;
        private readonly IRepository<Commentary> _commentaryRepository;
        private readonly IRepository<CommentaryBook> _commentaryBookRepository;
        private readonly IMapper<Commentary> _commentaryMapper;

        public CommentariesController(ILogger<CommentariesController> logger, IRepository<Commentary> commentaryRepository, IRepository<CommentaryBook> commentaryBookRepository, IMapper<Commentary> commentaryMapper)
        {
            _logger = logger;
            _commentaryRepository = commentaryRepository;
            _commentaryBookRepository = commentaryBookRepository;
            _commentaryMapper = commentaryMapper;
        }

        [HttpPost]
        public ActionResult<Commentary> Create([FromBody] CommentaryCreateModel commentaryCreateData)
        {
            if (commentaryCreateData.BookIds == null)
            {
                return BadRequest();
            }
            var commentary = new Commentary
            {
                Name = commentaryCreateData.Name,
                Isbn = commentaryCreateData.Isbn,
            };
            int newId = _commentaryRepository.Create(commentary);
            if (newId == 0)
            {
                return BadRequest();
            }
            commentary.Id = newId;

            foreach (var bookId in commentaryCreateData.BookIds)
            {
                _commentaryBookRepository.Create(new CommentaryBook
                {
                    BookId = bookId,
                    CommentaryId = newId,
                });
            }
            return Ok(commentary);
        }

        [HttpGet]
        public IEnumerable<Commentary> GetAll()
        {
            return _commentaryRepository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<Commentary> GetById(int id)
        {
            var result = _commentaryRepository.GetById(id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost("many")]
        public ActionResult<IEnumerable<Commentary>> Many([FromBody] IEnumerable<int> ids)
        {
            var commentaries = new List<Commentary>();
            try
            {
                using (var connection = new MySqlConnection(Startup.ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT * FROM Commentaries WHERE id in ({String.Join(", ", ids)})";
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
                            commentaries.Add(_commentaryMapper.FromFieldList(row));
                        }
                    }

                    if (commentaries.Count == 0)
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Commentary Book Controller (GetBooksForCommentary): {ex.Message}");
                return BadRequest();
            }
            return Ok(commentaries);
        }

        [HttpPut("{id}")]
        public Commentary Update(int id, [FromBody] Commentary commentary)
        {
            commentary.Id = id;
            return _commentaryRepository.Update(commentary);
        }
    }
}
