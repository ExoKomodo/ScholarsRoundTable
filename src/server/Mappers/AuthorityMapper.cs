using System;
using System.Collections.Generic;
using server.Models;

namespace server.Mappers
{
    public class AuthorityMapper : IMapper<Authority>
    {
        public Authority FromFieldList(IList<object> fields)
        {
            if (fields.Count < 3)
            {
                return null;
            }
            try
            {
                return new Authority
                {
                    Id = Convert.ToInt32(fields[0]),
                    Name = Convert.ToString(fields[1]),
                    SeminaryId = Convert.ToInt32(fields[2]),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authority Mapper: {ex.Message}");
                return null;
            }
        }
    }
}
