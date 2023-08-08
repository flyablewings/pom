using System;
using System.Collections;
using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.Entities.DHU;
using POMQC.Utilities;

namespace POMQC.Data.DHU
{
    public class DHURepository : IDHURepository
    {
        #region IDHURepository Members

        public IList<DHUEntity> Select(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0)
        {
            var result = new List<DHUEntity>();
            using (var sp = new StoredProcedure("sp_DHU_SelectByCustPO"))
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("custpo", custPO);
                param.Add("aiglpo", aiglPO);
                param.Add("type", type);
                param.Add("createddate", date);

                sp.AssignParamValues(param);
                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new DHUEntity
                            {
                                AgentId = reader["AgentId"].ConvertTo<int>(),
                                AIGLPO = reader["AIGLPO"].ConvertTo<string>(),
                                AuditSampleSize = reader["AuditSampleSize"].ConvertTo<int>(),
                                Comment = reader["Comment"].ConvertTo<string>(),
                                Country = reader["Country"].ConvertTo<string>(),
                                CreatedBy = reader["CreatedBy"].ConvertTo<int>(),
                                CreatedDate = reader["CreatedDate"].ConvertTo<DateTime>(),
                                CreateDate = reader["CreatedDate"].ConvertTo<DateTime>().ToString("MM/dd/yyyy HH:mm:ss.fff"),
                                UpdateDate = reader["UpdatedDate"].ConvertTo<DateTime>().ToString("MM/dd/yyyy HH:mm:ss.fff"),
                                CustId = reader["CustId"].ConvertTo<long>(),
                                CustPO = reader["CustPO"].ConvertTo<string>(),
                                DefectImg = reader["DefectImg"].ConvertTo<string>(),
                                DHUId = reader["DHUId"].ConvertTo<long>(),
                                DHUType = (DHUType)reader["DHUType"].ConvertTo<int>(),
                                FactoryId = reader["FactoryId"].ConvertTo<int>(),
                                Inspector = reader["Inspector"].ConvertTo<string>(),
                                LineNumber = reader["LineNumber"].ConvertTo<string>(),
                                OutputQty = reader["OutputQty"].ConvertTo<int>(),
                                POQty = reader["POQty"].ConvertTo<int>(),
                                Type = (DHUItem)reader["Type"].ConvertTo<int>(),
                                Type2 = (DHUStyle)reader["Type2"].ConvertTo<int>(),
                                UpdatedBy = reader["UpdatedBy"].ConvertTo<int>(),
                                UpdatedDate = reader["UpdatedDate"].ConvertTo<DateTime>(),
                                CreatedUser = reader["UserName"].ConvertTo<string>(),
                                UpdatedUser = reader["UpdatedUser"].ConvertTo<string>(),
                                AgentName = reader["AgentName"].ConvertTo<string>(),
                                FactoryName = reader["FactoryName"].ConvertTo<string>(),
                                CustName = reader["CustName"].ConvertTo<string>(),
                                Style = reader["Style"].ConvertTo<string>(),
                                Brand = reader["Brand"].ConvertTo<string>(),
                                InspectionId = reader["DHUId"].ConvertTo<long>()
                            });
                        }
                    }
                }
            }

            return result;
        }

        public Result Insert(DHUEntity entity)
        {
            using (var sp = new StoredProcedure("sp_DHU_Insert"))
            {
                var param = new Hashtable();
                param.Add("aiglpo", entity.AIGLPO);
                param.Add("auditsamplesize", entity.AuditSampleSize);
                param.Add("comment", entity.Comment);
                param.Add("country", entity.Country);
                param.Add("createdby", entity.CreatedBy);
                param.Add("createddate", entity.CreatedDate);
                param.Add("custid", entity.CustId);
                param.Add("custpo", entity.CustPO);
                param.Add("defectimg", entity.DefectImg);
                param.Add("dhutype", (int)entity.DHUType);
                param.Add("inspector", entity.Inspector);
                param.Add("linenumber", entity.LineNumber);
                param.Add("outputqty", entity.OutputQty);
                param.Add("poqty", entity.POQty);
                param.Add("type", (int)entity.Type);
                param.Add("type2", (int)entity.Type2);
                param.Add("dhuid", 0);
                param.Add("errmsg", string.Empty);
                param.Add("style", entity.Style);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                var id = sp.GetParameterValue("dhuid").ConvertTo<long>();
                var msg = sp.GetParameterValue("errmsg").ConvertTo<string>();

                return new Result { Id = id, Message = id > 0 ? "Created successfully." : msg };
            }
        }

        public Result Update(DHUEntity entity)
        {
            using (var sp = new StoredProcedure("sp_DHU_Update"))
            {
                var param = new Hashtable();
                param.Add("auditsamplesize", entity.AuditSampleSize);
                param.Add("comment", entity.Comment);
                param.Add("country", entity.Country);
                param.Add("updatedby", entity.UpdatedBy);
                param.Add("updateddate", entity.UpdatedDate);
                param.Add("defectimg", entity.DefectImg);
                param.Add("inspector", entity.Inspector);
                param.Add("linenumber", entity.LineNumber);
                param.Add("outputqty", entity.OutputQty);
                param.Add("poqty", entity.POQty);
                param.Add("type", (int)entity.Type);
                param.Add("type2", (int)entity.Type2);
                param.Add("dhuid", entity.DHUId);
                param.Add("style", entity.Style);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                return new Result { Id = entity.DHUId, Message = "Updated successfully." };
            }
        }

        public void Delete(long dhuId, string img)
        {
            using (var sp = new StoredProcedure("sp_DHU_DeleteImg"))
            {
                var param = new Hashtable();
                param.Add("img", img);
                param.Add("dhuid", dhuId);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();
            }
        }

        #endregion
    }
}