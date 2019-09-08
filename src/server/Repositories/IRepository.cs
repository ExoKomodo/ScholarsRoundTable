using System.Collections.Generic;

namespace server.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        int Create(T entity);
        T Update(T entity);
        bool Delete(int id);
    }
}