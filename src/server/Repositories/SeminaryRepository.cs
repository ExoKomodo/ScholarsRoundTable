using System.Collections.Generic;
using System.Linq;
using server.Helpers;
using server.Mappers;
using server.Models;

namespace server.Repositories
{
    public class SeminaryRepository : IRepository<Seminary>
    {
        public static readonly string TableName = "Seminaries";
        private readonly IMapper<Seminary> _seminaryMapper;

        public SeminaryRepository(IMapper<Seminary> seminaryMapper)
        {
            _seminaryMapper = seminaryMapper;
        }

        public int Create(Seminary entity) => RepositoryHelper.Create(TableName, new Dictionary<string, object>
        {
            ["name"] = entity.Name,
        });

        public bool Delete(int id) => RepositoryHelper.Delete(TableName, id);

        public IEnumerable<Seminary> GetAll() => RepositoryHelper.Get(TableName).Select(fields => _seminaryMapper.FromFieldList(fields));

        public Seminary GetById(int id) => _seminaryMapper.FromFieldList(RepositoryHelper.GetById(TableName, id));

        public Seminary Update(Seminary entity)
        {
            RepositoryHelper.Update(TableName, new Dictionary<string, object>
            {
                ["name"] = entity.Name,
            },
                entity.Id
            );
            return entity;
        }
    }
}
