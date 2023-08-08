using System;
using System.Collections;
using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.Entities.Defect;
using POMQC.Utilities;

namespace POMQC.Data.Report
{
    public class ReportRepository : IReportRepository
    {
        #region IReportRepository Members

        public IList<DefectDetail> ReportByCustPO(long custId, string custPO, string aiglPO)
        {
            var result = new List<DefectDetail>();
            using (var sp = new StoredProcedure("sp_Report_SelectByCustPO"))
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("custpo", custPO);
                param.Add("aiglpo", aiglPO);
                sp.AssignParamValues(param);

                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new DefectDetail
                            {
                                DefName = reader["DefName"].ConvertTo<string>(),
                                LocName = reader["LocName"].ConvertTo<string>(),
                                Type = (DefectType)reader["Type"].ConvertTo<int>(),
                                DHUType = (DHUType)reader["DHUType"].ConvertTo<int>(),
                                Total = reader["Total"].ConvertTo<int>(),
                            });
                        }
                    }
                }
            }

            return result;
        }

        public IList<DefectDetail> ReportByFactory(int factoryId, DateTime from, DateTime to)
        {
            var result = new List<DefectDetail>();
            using (var sp = new StoredProcedure("sp_Report_SelectByFactory"))
            {
                var param = new Hashtable();
                param.Add("factoryid", factoryId);
                param.Add("from", from);
                param.Add("to", to);
                sp.AssignParamValues(param);

                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new DefectDetail
                            {
                                DefName = reader["DefName"].ConvertTo<string>(),
                                LocName = reader["LocName"].ConvertTo<string>(),
                                Type = (DefectType)reader["Type"].ConvertTo<int>(),
                                DHUType = (DHUType)reader["DHUType"].ConvertTo<int>(),
                                Total = reader["Total"].ConvertTo<int>(),
                            });
                        }
                    }
                }
            }

            return result;
        }

        public IList<DefectDetail> ReportAllFactories(DateTime from, DateTime to)
        {
            var result = new List<DefectDetail>();
            using (var sp = new StoredProcedure("sp_Report_SelectAllFactories"))
            {
                var param = new Hashtable();
                param.Add("from", from);
                param.Add("to", to);
                sp.AssignParamValues(param);

                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new DefectDetail
                            {
                                DefName = reader["DefName"].ConvertTo<string>(),
                                LocName = reader["LocName"].ConvertTo<string>(),
                                Type = (DefectType)reader["Type"].ConvertTo<int>(),
                                DHUType = (DHUType)reader["DHUType"].ConvertTo<int>(),
                                Total = reader["Total"].ConvertTo<int>(),
                                FactoryName = reader["FactoryName"].ConvertTo<string>(),
                            });
                        }
                    }
                }
            }

            return result;
        }

        public IList<DefectDetail> ReportAllDetail(DateTime from, DateTime to)
        {
            var result = new List<DefectDetail>();
            using (var sp = new StoredProcedure("sp_Report_Detail"))
            {
                var param = new Hashtable();
                param.Add("from", from);
                param.Add("to", to);
                sp.AssignParamValues(param);

                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new DefectDetail
                            {
                                DefName = reader["DefName"].ConvertTo<string>(),
                                LocName = reader["LocName"].ConvertTo<string>(),
                                Type = (DefectType)reader["Type"].ConvertTo<int>(),
                                DHUType = (DHUType)reader["DHUType"].ConvertTo<int>(),
                                Total = reader["Total"].ConvertTo<int>(),
                                FactoryName = reader["FactoryName"].ConvertTo<string>(),
                                AIGLPO = reader["AIGLPO"].ConvertTo<string>(),
                                CustPO = reader["CustPO"].ConvertTo<string>(),
                                CustName = reader["CustName"].ConvertTo<string>(),
                                OrderQty = reader["OrderQty"].ConvertTo<int>(),
                                InspectedQty = reader["InspectedQty"].ConvertTo<int>(),
                                CreatedDate = reader["CreatedDate"].ConvertTo<DateTime>(),
                                CreatedUser = reader["UserName"].ConvertTo<string>(),
                            });
                        }
                    }
                }
            }

            return result;
        }

        public IList<WeeklyDHUReportEntity> ReportDHUWeekly(DateTime from, DateTime to)
        {
            var result = new List<WeeklyDHUReportEntity>();
            using (var sp = new StoredProcedure("sp_Report_DHU_Weekly"))
            {
                var param = new Hashtable();
                param.Add("from", from);
                param.Add("to", to);
                sp.AssignParamValues(param);

                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new WeeklyDHUReportEntity
                            {
                                FactoryName = reader["SupplierName"].ConvertTo<string>(),
                                Week1 = new WeeklyDHUEntity
                                {
                                    DefectPercent = reader["df_W1Per"].ConvertTo<int>(),
                                    DefectQty = reader["df_QTYW1"].ConvertTo<int>(),
                                    InspectionQty = reader["i_QTYW1"].ConvertTo<int>(),
                                    ProductPercent = reader["pd_W1Per"].ConvertTo<int>(),
                                    ProductQty= reader["pd_QTYW1"].ConvertTo<int>(),
                                },
                                Week2 = new WeeklyDHUEntity
                                {
                                    DefectPercent = reader["df_W2Per"].ConvertTo<int>(),
                                    DefectQty = reader["df_QTYW2"].ConvertTo<int>(),
                                    InspectionQty = reader["i_QTYW2"].ConvertTo<int>(),
                                    ProductPercent = reader["pd_W2Per"].ConvertTo<int>(),
                                    ProductQty = reader["pd_QTYW2"].ConvertTo<int>(),
                                },
                                Week3 = new WeeklyDHUEntity
                                {
                                    DefectPercent = reader["df_W3Per"].ConvertTo<int>(),
                                    DefectQty = reader["df_QTYW3"].ConvertTo<int>(),
                                    InspectionQty = reader["i_QTYW3"].ConvertTo<int>(),
                                    ProductPercent = reader["pd_W3Per"].ConvertTo<int>(),
                                    ProductQty = reader["pd_QTYW3"].ConvertTo<int>(),
                                },
                                Week4 = new WeeklyDHUEntity
                                {
                                    DefectPercent = reader["df_W4Per"].ConvertTo<int>(),
                                    DefectQty = reader["df_QTYW4"].ConvertTo<int>(),
                                    InspectionQty = reader["i_QTYW4"].ConvertTo<int>(),
                                    ProductPercent = reader["pd_W4Per"].ConvertTo<int>(),
                                    ProductQty = reader["pd_QTYW4"].ConvertTo<int>(),
                                }
                            });
                        }
                    }
                }
                
            }

            return result;
        }

        public IList<MonthlyDHUReportEntity> ReportDHUMonthly(DateTime from, DateTime to)
        {
            var result = new List<MonthlyDHUReportEntity>();
            using (var sp = new StoredProcedure("sp_Report_DHU_Monthly"))
            {
                var param = new Hashtable();
                param.Add("from", from);
                param.Add("to", to);
                sp.AssignParamValues(param);

                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new MonthlyDHUReportEntity
                            {
                                FactoryName = reader["SupplierName"].ConvertTo<string>(),
                                DefectPercent = reader["df_Percent"].ConvertTo<int>(),
                                DefectQty = reader["df_QTY"].ConvertTo<int>(),
                                InspectionQty = reader["i_QTY"].ConvertTo<int>(),
                                ProductPercent = reader["pd_Percent"].ConvertTo<int>(),
                                ProductQty = reader["pd_QTY"].ConvertTo<int>(),
                                OutputQty = reader["Output_QTY"].ConvertTo<int>(),
                                Month = reader["Month"].ConvertTo<int>(),
                                Year = reader["Year"].ConvertTo<int>()
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