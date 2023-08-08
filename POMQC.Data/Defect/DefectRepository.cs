using System;
using System.Collections;
using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.Entities.Defect;
using POMQC.Utilities;

namespace POMQC.Data.Defect
{
    public class DefectRepository : IDefectRepository
    {
        #region IDefectRepository Members

        public IList<DefectCode> DefectCodes()
        {
            var result = new List<DefectCode>();
            using (var sp = new StoredProcedure("sp_DefectCode_SelectAll"))
            {
                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new DefectCode
                            {
                                CatId = reader["CatId"].ConvertTo<int>(),
                                DefId = reader["DefId"].ConvertTo<int>(),
                                DefCode = reader["DefCode"].ConvertTo<string>(),
                                DefName = reader["DefName"].ConvertTo<string>(),
                                DefVN = reader["DefVN"].ConvertTo<string>(),
                                Type = (DefectType)reader["Type"].ConvertTo<int>(),
                                CatName = reader["CatName"].ConvertTo<string>(),
                                Active = reader["Active"].ConvertTo<bool>(),
                                CreatedBy = reader["CreatedBy"].ConvertTo<int>(),
                                CreatedDate = reader["CreatedDate"].ConvertTo<DateTime>(),
                                CreatedUser = reader["CreatedUser"].ConvertTo<string>(),
                                UpdatedBy = reader["UpdatedBy"].ConvertTo<int>(),
                                UpdatedDate = reader["UpdatedDate"].ConvertTo<DateTime>(),
                                UpdatedUser = reader["UpdatedUser"].ConvertTo<string>()
                            });
                        }
                    }
                }
            }

            return result;
        }

        public IList<DefectLocation> DefectionLocations()
        {
            var result = new List<DefectLocation>();
            using (var sp = new StoredProcedure("sp_DefectLocation_SelectAll"))
            {
                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new DefectLocation
                            {
                                LocId = reader["LocId"].ConvertTo<int>(),
                                LocName = reader["LocName"].ConvertTo<string>(),
                                Active = reader["Active"].ConvertTo<bool>(),
                                CreatedBy = reader["CreatedBy"].ConvertTo<int>(),
                                CreatedDate = reader["CreatedDate"].ConvertTo<DateTime>(),
                                CreatedUser = reader["CreatedUser"].ConvertTo<string>(),
                                UpdatedBy = reader["UpdatedBy"].ConvertTo<int>(),
                                UpdatedDate = reader["UpdatedDate"].ConvertTo<DateTime>(),
                                UpdatedUser = reader["UpdatedUser"].ConvertTo<string>()
                            });
                        }
                    }
                }
            }

            return result;
        }

        public Result InsertDetail(DefectDetail item)
        {
            using (var sp = new StoredProcedure("sp_DefectDetail_Insert"))
            {
                var param = new Hashtable();
                param.Add("custid", item.CustId);
                param.Add("custpo", item.CustPO);
                param.Add("aiglpo", item.AIGLPO);
                param.Add("factoryid", item.FactoryId);
                param.Add("agentid", item.AgentId);
                param.Add("inspectionid", item.InspectionId);
                param.Add("defid", item.DefId);
                param.Add("locid", item.LocId);
                param.Add("type", (int)item.DHUType);
                param.Add("createddate", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                param.Add("pcsqty", item.PCSQty);
                param.Add("total", item.Total);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                return new Result { Id = 1 };
            }
        }

        public Result UpdateDetail(DefectDetail item)
        {
            using (var sp = new StoredProcedure("sp_DefectDetail_Update"))
            {
                var param = new Hashtable();
                param.Add("custid", item.CustId);
                param.Add("custpo", item.CustPO);
                param.Add("aiglpo", item.AIGLPO);
                param.Add("inspectionid", item.InspectionId);
                param.Add("defid", item.DefId);
                param.Add("locid", item.LocId);
                param.Add("type", (int)item.DHUType);
                param.Add("pcsqty", item.PCSQty);
                param.Add("total", item.Total);
                param.Add("ddefid", item.DdefId);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                return new Result { Id = item.DdefId };
            }
        }

        public IList<DefectDetail> DefectDetails(long custId, string custPO, string AIGLPO, int Type)
        {
            var result = new List<DefectDetail>();

            using (var sp = new StoredProcedure("sp_DefectDetail_SelectByCustPO"))
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("custpo", custPO);
                param.Add("aiglpo", AIGLPO);
                param.Add("type", Type);

                sp.AssignParamValues(param);
                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new DefectDetail
                            {
                                InspectionId = reader["InspectionId"].ConvertTo<long>(),
                                DefId = reader["DefId"].ConvertTo<int>(),
                                LocId = reader["LocId"].ConvertTo<int>(),
                                DHUType = (DHUType)reader["Type"].ConvertTo<int>(),
                                PCSQty = reader["PCSQty"].ConvertTo<int>(),
                                Total = reader["Total"].ConvertTo<int>(),
                                AgentId = reader["AgentId"].ConvertTo<int>(),
                                CustId = reader["CustId"].ConvertTo<long>(),
                                FactoryId = reader["FactoryId"].ConvertTo<int>(),
                                CreatedDate = reader["CreatedDate"].ConvertTo<DateTime>(),
                                CustPO = reader["CustPO"].ConvertTo<string>(),
                                AIGLPO = reader["AIGLPO"].ConvertTo<string>(),
                                DdefId = reader["DdefId"].ConvertTo<long>(),
                                Type = (DefectType)reader["DefectType"].ConvertTo<int>()
                            });
                        }
                    }
                }
            }

            return result;
        }
        
        public IList<DefectCategory> DefectCategories()
        {
            var result = new List<DefectCategory>();
            using (var sp = new StoredProcedure("sp_DefectCategory_SelectAll"))
            {
                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new DefectCategory
                            {
                                Active = reader["Active"].ConvertTo<bool>(),
                                CatId = reader["CatId"].ConvertTo<int>(),
                                CatName = reader["CatName"].ConvertTo<string>(),
                                CreatedBy = reader["CreatedBy"].ConvertTo<int>(),
                                CreatedDate = reader["CreatedDate"].ConvertTo<DateTime>(),
                                CreatedUser = reader["CreatedUser"].ConvertTo<string>(),
                                UpdatedBy = reader["UpdatedBy"].ConvertTo<int>(),
                                UpdatedDate = reader["UpdatedDate"].ConvertTo<DateTime>(),
                                UpdatedUser = reader["UpdatedUser"].ConvertTo<string>()
                            });
                        }
                    }
                }
            }

            return result;
        }

        public Result InsertCategory(DefectCategory item)
        {
            using (var sp = new StoredProcedure("sp_DefectCategory_Insert"))
            {
                var param = new Hashtable();
                param.Add("catname", item.CatName);
                param.Add("active", item.Active);
                param.Add("catid", item.CatId);
                param.Add("createdby", item.CreatedBy);
                param.Add("createddate", item.CreatedDate);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                var catId = sp.GetParameterValue("catid").ConvertTo<int>();
                return new Result { Id = catId, Message = catId > 0 ? "Created successfully" : "An error occurs, please check network connection or try again later" };
            }
        }

        public Result UpdateCategory(DefectCategory item)
        {
            using (var sp = new StoredProcedure("sp_DefectCategory_Update"))
            {
                var param = new Hashtable();
                param.Add("catname", item.CatName);
                param.Add("active", item.Active);
                param.Add("catid", item.CatId);
                param.Add("updatedby", item.UpdatedBy);
                param.Add("updateddate", item.UpdatedDate);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                var catId = item.CatId;
                return new Result { Id = catId, Message = catId > 0 ? "Updated successfully" : "An error occurs, please check network connection or try again later" };
            }
        }

        public Result InsertLocation(DefectLocation item)
        {
            using (var sp = new StoredProcedure("sp_DefectLocation_Insert"))
            {
                var param = new Hashtable();
                param.Add("locname", item.LocName);
                param.Add("active", item.Active);
                param.Add("locid", item.LocId);
                param.Add("createdby", item.CreatedBy);
                param.Add("createddate", item.CreatedDate);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                var locId = sp.GetParameterValue("locid").ConvertTo<int>();
                return new Result { Id = locId, Message = locId > 0 ? "Created successfully" : "An error occurs, please check network connection or try again later" };
            }
        }

        public Result UpdateLocation(DefectLocation item)
        {
            using (var sp = new StoredProcedure("sp_DefectLocation_Update"))
            {
                var param = new Hashtable();
                param.Add("locname", item.LocName);
                param.Add("active", item.Active);
                param.Add("locid", item.LocId);
                param.Add("updatedby", item.UpdatedBy);
                param.Add("updateddate", item.UpdatedDate);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                var locId = item.LocId;
                return new Result { Id = locId, Message = locId > 0 ? "Updated successfully" : "An error occurs, please check network connection or try again later" };
            }
        }
        
        public Result InsertCode(DefectCode item)
        {
            using (var sp = new StoredProcedure("sp_DefectCode_Insert"))
            {
                var param = new Hashtable();
                param.Add("defcode", item.DefCode);
                param.Add("defname", item.DefName);
                param.Add("defvn", item.DefVN);
                param.Add("active", item.Active);
                param.Add("defid", item.DefId);
                param.Add("catid", item.CatId);
                param.Add("createdby", item.CreatedBy);
                param.Add("createddate", item.CreatedDate);
                param.Add("type", (int)item.Type);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                var defId = sp.GetParameterValue("defid").ConvertTo<int>();
                return new Result { Id = defId, Message = defId > 0 ? "Created successfully" : "An error occurs, please check network connection or try again later" };
            }
        }

        public Result UpdateCode(DefectCode item)
        {
            using (var sp = new StoredProcedure("sp_DefectCode_Update"))
            {
                var param = new Hashtable();
                param.Add("defcode", item.DefCode);
                param.Add("defname", item.DefName);
                param.Add("defvn", item.DefVN);
                param.Add("active", item.Active);
                param.Add("defid", item.DefId);
                param.Add("catid", item.CatId);
                param.Add("updatedby", item.UpdatedBy);
                param.Add("updateddate", item.UpdatedDate);
                param.Add("type", (int)item.Type);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                var defId = item.DefId;
                return new Result { Id = defId, Message = defId > 0 ? "Updated successfully" : "An error occurs, please check network connection or try again later" };
            }
        }

        #endregion
    }
}