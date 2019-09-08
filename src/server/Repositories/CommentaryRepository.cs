using System.Collections.Generic;
using System.Linq;
using server.Helpers;
using server.Mappers;
using server.Models;

namespace server.Repositories
{
    public class CommentaryRepository : IRepository<Commentary>
    {
        public static readonly string TableName = "Commentaries";
        private readonly IMapper<Commentary> _commentaryMapper;

        public CommentaryRepository(IMapper<Commentary> commentaryMapper)
        {
            _commentaryMapper = commentaryMapper;
        }

        public int Create(Commentary entity) => RepositoryHelper.Create(TableName, new Dictionary<string, object>
        {
            ["name"] = entity.Name,
            ["isbn"] = entity.Isbn,
        });

        public bool Delete(int id) => RepositoryHelper.Delete(TableName, id);

        public IEnumerable<Commentary> GetAll() => RepositoryHelper.Get(TableName).Select(fields => _commentaryMapper.FromFieldList(fields));

        public Commentary GetById(int id) => _commentaryMapper.FromFieldList(RepositoryHelper.GetById(TableName, id));

        public Commentary Update(Commentary entity)
        {
            RepositoryHelper.Update(TableName, new Dictionary<string, object>
            {
                ["name"] = entity.Name,
                ["isbn"] = entity.Isbn,
            },
                entity.Id
            );
            return entity;
        }
    }
}
