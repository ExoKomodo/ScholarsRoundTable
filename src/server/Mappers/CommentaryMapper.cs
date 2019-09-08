using System;
using System.Collections.Generic;
using server.Models;

namespace server.Mappers
{
    public class CommentaryMapper : IMapper<Commentary>
    {
        public Commentary FromFieldList(IList<object> fields)
        {
            if (fields.Count < 3)
            {
                return null;
            }
            try
            {
                return new Commentary
                {
                    Id = Convert.ToInt32(fields[0]),
                    Name = Convert.ToString(fields[1]),
                    Isbn = Convert.ToString(fields[2]),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Commentary Mapper: {ex.Message}");
                return null;
            }
        }
    }
}
