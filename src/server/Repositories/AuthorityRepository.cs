using System.Collections.Generic;
using System.Linq;
using server.Helpers;
using server.Mappers;
using server.Models;

namespace server.Repositories
{
    public class AuthorityRepository : IRepository<Authority>
    {
        public static readonly string TableName = "Authorities";
        private readonly IMapper<Authority> _authorityMapper;

        public AuthorityRepository(IMapper<Authority> authorityMapper)
        {
            _authorityMapper = authorityMapper;
        }

        public int Create(Authority entity) => RepositoryHelper.Create(TableName, new Dictionary<string, object>
        {
            ["name"] = entity.Name,
            ["seminary_id"] = entity.SeminaryId,
        });

        public bool Delete(int id) => RepositoryHelper.Delete(TableName, id);

        public IEnumerable<Authority> GetAll() => RepositoryHelper.Get(TableName).Select(fields => _authorityMapper.FromFieldList(fields));

        public Authority GetById(int id) => _authorityMapper.FromFieldList(RepositoryHelper.GetById(TableName, id));

        public Authority Update(Authority entity)
        {
            RepositoryHelper.Update(TableName, new Dictionary<string, object>
            {
                ["name"] = entity.Name,
                ["seminary_id"] = entity.SeminaryId,
            },
                entity.Id
            );
            return entity;
        }
    }
}
