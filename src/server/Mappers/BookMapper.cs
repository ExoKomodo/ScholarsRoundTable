using System;
using System.Collections.Generic;
using server.Models;

namespace server.Mappers
{
    public class BookMapper : IMapper<Book>
    {
        public Book FromFieldList(IList<object> fields)
        {
            if (fields.Count < 2)
            {
                return null;
            }
            try
            {
                return new Book
                {
                    Id = Convert.ToInt32(fields[0]),
                    Name = Convert.ToString(fields[1]),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Book Mapper: {ex.Message}");
                return null;
            }
        }
    }
}
