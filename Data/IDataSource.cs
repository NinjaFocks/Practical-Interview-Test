﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rdessoy_MCMS_Practical_Interview_Test.Data;

public interface IDataSource
{
    Task<IList<HashModel>> GetHashModelsAsync(int? pageSize = null, int pageNumber = 1);

    Task<int> GetPageCountAsync();
}
