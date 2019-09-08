using System;
using System.Collections.Generic;
using server.Models;

namespace server.Mappers
{
    public class RankingMapper : IMapper<Ranking>
    {
        public Ranking FromFieldList(IList<object> fields)
        {
            if (fields.Count < 4)
            {
                return null;
            }
            try
            {
                return new Ranking
                {
                    AuthorityId = Convert.ToInt32(fields[0]),
                    CommentaryId = Convert.ToInt32(fields[1]),
                    BookId = Convert.ToInt32(fields[2]),
                    Rank = (float)Convert.ToDouble(fields[3]),
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ranking Mapper: {ex.Message}");
                return null;
            }
        }
    }
}
