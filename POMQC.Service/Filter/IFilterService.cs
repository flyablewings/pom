using System.Collections.Generic;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Filter;

namespace POMQC.Services.Filter
{
    public interface IFilterService
    {
        FilterViewModel Filters(bool showAllLabel, bool showNewButton, bool showViewButton, FunctionViewModel function, bool showDateRange = false, bool showStatus = true, bool showFactory = true);

        IList<FactoryBase> Factories();
    }
}