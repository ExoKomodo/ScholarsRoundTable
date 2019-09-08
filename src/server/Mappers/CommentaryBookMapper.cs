using System;
using System.Collections.Generic;
using server.Models;

namespace server.Mappers
{
    public class CommentaryBookMapper : IMapper<CommentaryBook>
    {
        public CommentaryBook FromFieldList(IList<object> fields)
        {
            if (fields.Count < 2)
            {
                return null;
            }
            try
            {
                return new CommentaryBook
                {
                    BookId = Convert.ToInt32(fields[0]),
                    CommentaryId = Convert.ToInt32(fields[1]),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Commentary Book Mapper: {ex.Message}");
                return null;
            }
        }
    }
}
