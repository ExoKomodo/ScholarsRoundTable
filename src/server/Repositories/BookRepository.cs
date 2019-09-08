using System.Collections.Generic;
using System;
using System.Linq;
using server.Helpers;
using server.Mappers;
using server.Models;

namespace server.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        public static readonly string TableName = "Books";
        private readonly IMapper<Book> _bookMapper;

        public BookRepository(IMapper<Book> bookMapper)
        {
            _bookMapper = bookMapper;
        }

        public int Create(Book entity) => throw new NotSupportedException();

        public bool Delete(int id) => throw new NotSupportedException();

        public IEnumerable<Book> GetAll() => RepositoryHelper.Get(TableName).Select(fields => _bookMapper.FromFieldList(fields));

        public Book GetById(int id) => _bookMapper.FromFieldList(RepositoryHelper.GetById(TableName, id));

        public Book Update(Book entity) => throw new NotSupportedException();
    }
}
