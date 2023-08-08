using System.Collections;
using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.Entities.Checklist;
using POMQC.Utilities;
using System;

namespace POMQC.Data.Checklist
{
    public class ChecklistRepository : IChecklistRepository
    {
        #region IChecklistRepository Members

        public Result Insert(ChecklistEntity entity)
        {
            using (var sp = new StoredProcedure("sp_Checklist_Insert"))
            {
                var param = new Hashtable();
                param.Add("aiglpo", entity.AIGLPO);
                param.Add("comment", entity.Comment);
                param.Add("createdby", entity.CreatedBy);
                param.Add("createddate", entity.CreatedDate);
                param.Add("custid", entity.CustId);
                param.Add("custpo", entity.CustPO);
                param.Add("doc", entity.Doc);
                param.Add("type", (int)entity.Type);
                param.Add("errmsg", string.Empty);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                var message = sp.GetParameterValue("errmsg").ConvertTo<string>();
                var success = string.IsNullOrWhiteSpace(message);

                return new Result 
                { 
                    Id = success ? 1 : 0,
                    Message = success ? "Created successfully." : message
                };
            }
        }

        public Result Update(ChecklistEntity entity)
        {
            using (var sp = new StoredProcedure("sp_Checklist_Update"))
            {
                var param = new Hashtable();
                param.Add("aiglpo", entity.AIGLPO);
                param.Add("comment", entity.Comment);
                param.Add("updatedby", entity.UpdatedBy);
                param.Add("updateddate", entity.UpdatedDate);
                param.Add("custid", entity.CustId);
                param.Add("custpo", entity.CustPO);
                param.Add("doc", entity.Doc);
                param.Add("type", (int)entity.Type);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();
                
                return new Result
                {
                    Id = entity.CustId,
                    Message = "Updated successfully."
                };
            }
        }

        public IList<ChecklistEntity> Select(long custId, string custPO, string aiglPO, int type = 0)
        {
            var result = new List<ChecklistEntity>();
            using (var sp = new StoredProcedure("sp_Checklist_SelectByCustPO"))
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("custpo", custPO);
                param.Add("aiglpo", aiglPO);
                param.Add("type", type);

                sp.AssignParamValues(param);
                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null) 
                    {
                        while (reader.Read())
                        {
                            result.Add(new ChecklistEntity
                            {
                                Type = (ChecklistType)reader["Type"].ConvertTo<int>(),
                                Doc = reader["Doc"].ConvertTo<string>(),
                                Comment = reader["Comment"].ConvertTo<string>(),
                                CustId = reader["CustId"].ConvertTo<long>(),
                                CustPO = reader["CustPO"].ConvertTo<string>(),
                                AIGLPO = reader["AIGLPO"].ConvertTo<string>(),
                                CreatedDate = reader["CreatedDate"].ConvertTo<DateTime>(),
                                CreatedUser = reader["CreatedUser"].ConvertTo<string>(),
                                UpdatedDate = reader["UpdatedDate"].ConvertTo<DateTime>(),
                                UpdatedUser = reader["UpdatedUser"].ConvertTo<string>()
                            });
                        }
                    }
                }
            }

            return result;
        }

        public void Delete(long custId, string custPO, string aiglPO, string doc, int type)
        {
            using (var sp = new StoredProcedure("sp_Checklist_DeleteImg"))
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("custpo", custPO);
                param.Add("aiglpo", aiglPO);
                param.Add("doc", doc);
                param.Add("type", type);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();
            }
        }

        #endregion
    }
}