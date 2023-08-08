using System.Collections.Generic;
using POMQC.Entities.Filter;
using POMQC.Utilities;

namespace POMQC.Data.Filter
{
    public class FilterRepository : IFilterRepository
    {
        #region IFilterRepository Members

        public IList<CustomerFilterEntity> Customers()
        {
            var result = new List<CustomerFilterEntity>();
            using (var sp = new StoredProcedure("sp_Customer_SelectAll"))
            {
                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new CustomerFilterEntity 
                            { 
                                CustId = reader["CustId"].ConvertTo<long>(), 
                                CustName = reader["CustName"].ConvertTo<string>(),
                                Parent = reader["FactoryId"].ConvertTo<string>()
                            });
                        }
                    }
                }
            }

            return result;
        }

        public IList<POFilterEntity> CustPOs()
        {
            var result = new List<POFilterEntity>();
            using (var sp = new StoredProcedure("sp_CustPO_SelectAll"))
            {
                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new POFilterEntity { Parent = reader["Parent"].ConvertTo<string>(), PO = reader["PO"].ConvertTo<string>().Trim() });
                        }
                    }
                }
            }

            return result;
        }

        public IList<POFilterEntity> AIGLPOs()
        {
            var result = new List<POFilterEntity>();
            using (var sp = new StoredProcedure("sp_AIGLPO_SelectAll"))
            {
                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new POFilterEntity { Parent = reader["Parent"].ConvertTo<string>(), PO = reader["PO"].ConvertTo<string>().Trim() });
                        }
                    }
                }
            }

            return result;
        }

        public IList<FactoryFilterEntity> Factories()
        {
            var result = new List<FactoryFilterEntity>();
            using (var sp = new StoredProcedure("sp_Factory_SelectAll"))
            {
                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new FactoryFilterEntity { FactoryId = reader["FactoryId"].ConvertTo<int>(), FactoryName = reader["FactoryName"].ConvertTo<string>() });
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
