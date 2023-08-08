using System.Collections.Generic;
using POMQC.Entities.Filter;

namespace POMQC.Data.Filter
{
    public interface IFilterRepository
    {
        IList<CustomerFilterEntity> Customers();

        IList<POFilterEntity> CustPOs();

        IList<POFilterEntity> AIGLPOs();

        IList<FactoryFilterEntity> Factories();
    }
}