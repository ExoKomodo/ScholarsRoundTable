using System.Collections.Generic;

namespace server.Mappers
{
    public interface IMapper<T>
    {
        T FromFieldList(IList<object> fields);
    }
}
