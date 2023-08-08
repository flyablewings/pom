using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POMQC.Entities;
using System.Collections;
using POMQC.Utilities;

namespace POMQC.Data.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        #region IDashboardRepository Members

        public IList<POInfoEntity> POs(DateTime from, DateTime to, long custId = 0, string custPO = "", string aiglPO = "")
        {
            var result = new List<POInfoEntity>();
            using (var sp = new StoredProcedure("sp_PO_SelectByCustPO"))
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("custpo", custPO);
                param.Add("aiglpo", aiglPO);
                param.Add("from", from);
                param.Add("to", to);

                sp.AssignParamValues(param);
                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new POInfoEntity
                            {
                                AIGLPO = reader["AIGLPO"].ConvertTo<string>(),
                                Customer = reader["CustName"].ConvertTo<string>(),
                                CustPO = reader["CustPO"].ConvertTo<string>(),
                                Defect = reader["Defects"].ConvertTo<int>(),
                                Division = reader["Division"].ConvertTo<string>(),
                                Factory = reader["FactoryName"].ConvertTo<string>(),
                                CustId = reader["CustId"].ConvertTo<long>(),
                                InlineDate = reader["InlineDate"].ConvertTo<DateTime>().ToString("MM/dd/yyyy"),
                                Status = reader["Status"].ConvertTo<string>(),
                                POQty = reader["POQty"].ConvertTo<int>(),
                                FactoryId = reader["FactoryId"].ConvertTo<int>()
                            });
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
