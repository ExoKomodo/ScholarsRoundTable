using System;
using System.Collections.Generic;
using server.Models;

namespace server.Mappers
{
    public class SeminaryMapper : IMapper<Seminary>
    {
        public Seminary FromFieldList(IList<object> fields)
        {
            if (fields.Count < 2)
            {
                return null;
            }
            try
            {
                return new Seminary
                {
                    Id = Convert.ToInt32(fields[0]),
                    Name = Convert.ToString(fields[1]),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seminary Mapper: {ex.Message}");
                return null;
            }
        }
    }
}
