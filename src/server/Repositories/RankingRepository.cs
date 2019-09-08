using System;
using System.Collections.Generic;
using System.Linq;
using server.Helpers;
using server.Mappers;
using server.Models;

namespace server.Repositories
{
    public class RankingRepository : IRepository<Ranking>
    {
        public static readonly string TableName = "Rankings";
        private readonly IMapper<Ranking> _rankingMapper;

        public RankingRepository(IMapper<Ranking> rankingMapper)
        {
            _rankingMapper = rankingMapper;
        }

        public int Create(Ranking entity)
        {
            try
            {
                RepositoryHelper.Create(TableName, new Dictionary<string, object>
                {
                    ["authority_id"] = entity.AuthorityId,
                    ["book_id"] = entity.BookId,
                    ["commentary_id"] = entity.CommentaryId,
                    ["ranking"] = entity.Rank,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ranking Repository (Create): {ex.Message}");
                return 0;
            }
            return 1;
        }

        public bool Delete(int id) => throw new NotSupportedException();

        public IEnumerable<Ranking> GetAll() => throw new NotSupportedException();

        public Ranking GetById(int id) => throw new NotSupportedException();

        public Ranking Update(Ranking entity) => throw new NotSupportedException();
    }
}
