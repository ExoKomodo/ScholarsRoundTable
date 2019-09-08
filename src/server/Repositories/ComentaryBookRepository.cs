using System;
using System.Collections.Generic;
using System.Linq;
using server.Helpers;
using server.Mappers;
using server.Models;

namespace server.Repositories
{
    public class CommentaryBookRepository : IRepository<CommentaryBook>
    {
        public static readonly string TableName = "CommentaryBook";
        private readonly IMapper<CommentaryBook> _commentaryBookMapper;

        public CommentaryBookRepository(IMapper<CommentaryBook> commentaryBookMapper)
        {
            _commentaryBookMapper = commentaryBookMapper;
        }

        public int Create(CommentaryBook entity) => RepositoryHelper.Create(TableName, new Dictionary<string, object>
        {
            ["book_id"] = entity.BookId,
            ["commentary_id"] = entity.CommentaryId,
        });

        public bool Delete(int id) => RepositoryHelper.Delete(TableName, id);

        public IEnumerable<CommentaryBook> GetAll() => RepositoryHelper.Get(TableName).Select(fields => _commentaryBookMapper.FromFieldList(fields));

        public CommentaryBook GetById(int id) => _commentaryBookMapper.FromFieldList(RepositoryHelper.GetById(TableName, id));

        public CommentaryBook Update(CommentaryBook entity)
        {
            throw new NotSupportedException();
        }
    }
}
