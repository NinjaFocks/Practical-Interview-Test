using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rdessoy_MCMS_Practical_Interview_Test.Data;

public interface IDataSource
{
    Task<KeyValuePair<int, IList<HashModel>>> GetHashModelsAsync(string? search = null, int? pageSize = null, int pageNumber = 1);

    Task<int> GetPageCountAsync(string? query = null);
}
