using System;
using System.Collections;
using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.Entities.Final;
using POMQC.Utilities;

namespace POMQC.Data.Final
{
    public class FinalRepository : IFinalRepository
    {
        #region IFinalRepository Members

        public IList<FinalEntity> Select(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0)
        {
            var result = new List<FinalEntity>();
            using (var sp = new StoredProcedure("sp_Final_SelectByCustPO"))
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
                            result.Add(new FinalEntity
                            {
                                AgentId = reader["AgentId"].ConvertTo<int>(),
                                AIGLPO = reader["AIGLPO"].ConvertTo<string>(),
                                CreatedBy = reader["CreatedBy"].ConvertTo<int>(),
                                CreatedDate = reader["CreatedDate"].ConvertTo<DateTime>(),
                                CreateDate = reader["CreatedDate"].ConvertTo<DateTime>().ToString("MM/dd/yyyy HH:mm:ss.fff"),
                                UpdateDate = reader["UpdatedDate"].ConvertTo<DateTime>().ToString("MM/dd/yyyy HH:mm:ss.fff"),
                                CustId = reader["CustId"].ConvertTo<long>(),
                                CustPO = reader["CustPO"].ConvertTo<string>(),
                                FactoryId = reader["FactoryId"].ConvertTo<int>(),
                                UpdatedBy = reader["UpdatedBy"].ConvertTo<int>(),
                                UpdatedDate = reader["UpdatedDate"].ConvertTo<DateTime>(),
                                FactoryName = reader["FactoryName"].ConvertTo<string>(),
                                CustName = reader["CustName"].ConvertTo<string>(),
                                AuditedBy = reader["AuditedBy"].ConvertTo<string>(),
                                CartonComment = reader["CartonComment"].ConvertTo<string>(),
                                Comment = reader["Comment"].ConvertTo<string>(),
                                FactoryCaption = reader["FactoryCaption"].ConvertTo<string>(),
                                FactoryManager = reader["FactoryManager"].ConvertTo<string>(),
                                FactoryRep = reader["FactoryRep"].ConvertTo<string>(),
                                FinalComment = reader["FinalComment"].ConvertTo<string>(),
                                FinalId = reader["FinalId"].ConvertTo<long>(),
                                InspectionId = reader["FinalId"].ConvertTo<long>(),
                                FinalStatus = (StatusType)reader["FinalStatus"].ConvertTo<int>(),
                                MainLabel = reader["MainLabel"].ConvertTo<string>(),
                                WorkmanshipAQL = (AQL)reader["AQL"].ConvertTo<int>(),
                                MeasurementAQL = (AQL)reader["AQL2"].ConvertTo<int>(),
                                MeasurementComment = reader["MeasurementComment"].ConvertTo<string>(),
                                MeasurementPackingAudit = reader["MeasurementPackingAudit"].ConvertTo<string>(),
                                MeasurementStatus = (StatusType)reader["MeasurementStatus"].ConvertTo<int>(),
                                PackingComment = reader["PackingComment"].ConvertTo<string>(),
                                PackingStatus = (StatusType)reader["PackingStatus"].ConvertTo<int>(),
                                QAAuditor = reader["QAAuditor"].ConvertTo<string>(),
                                QAComment = reader["QAComment"].ConvertTo<string>(),
                                QAManager = reader["QAManager"].ConvertTo<string>(),
                                Type = (DHUType)reader["Type"].ConvertTo<int>(),
                                WorkmanshipComment = reader["WorkmanshipComment"].ConvertTo<string>(),
                                WorkmanshipStatus = (StatusType)reader["WorkmanshipStatus"].ConvertTo<int>(),
                                CreatedUser = reader["UserName"].ConvertTo<string>(),
                                UpdatedUser = reader["UpdatedUser"].ConvertTo<string>(),
                                ActualInspectionDate = reader["ActualInspectionDate"].ConvertTo<DateTime>().ToString("MM/dd/yyyy HH:mm:ss.fff")
                            });
                        }
                    }
                }
            }

            return result;
        }

        public IList<FinalEntity> SelectAll(DateTime from, DateTime to)
        {
            var result = new List<FinalEntity>();
            using (var sp = new StoredProcedure("sp_Report_SelectAllFinal"))
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
                            var entity = new FinalEntity
                            {
                                AgentId = reader["AgentId"].ConvertTo<int>(),
                                AIGLPO = reader["AIGLPO"].ConvertTo<string>(),
                                CreatedBy = reader["CreatedBy"].ConvertTo<int>(),
                                CreatedDate = reader["CreatedDate"].ConvertTo<DateTime>(),
                                CreateDate = reader["CreatedDate"].ConvertTo<DateTime>().ToString("MM/dd/yyyy HH:mm:ss.fff"),
                                UpdateDate = reader["UpdatedDate"].ConvertTo<DateTime>().ToString("MM/dd/yyyy HH:mm:ss.fff"),
                                CustId = reader["CustId"].ConvertTo<long>(),
                                CustPO = reader["CustPO"].ConvertTo<string>(),
                                FactoryId = reader["FactoryId"].ConvertTo<int>(),
                                UpdatedBy = reader["UpdatedBy"].ConvertTo<int>(),
                                UpdatedDate = reader["UpdatedDate"].ConvertTo<DateTime>(),
                                FactoryName = reader["FactoryName"].ConvertTo<string>(),
                                CustName = reader["CustName"].ConvertTo<string>(),
                                AuditedBy = reader["AuditedBy"].ConvertTo<string>(),
                                CartonComment = reader["CartonComment"].ConvertTo<string>(),
                                Comment = reader["Comment"].ConvertTo<string>(),
                                FactoryCaption = reader["FactoryCaption"].ConvertTo<string>(),
                                FactoryManager = reader["FactoryManager"].ConvertTo<string>(),
                                FactoryRep = reader["FactoryRep"].ConvertTo<string>(),
                                FinalComment = reader["FinalComment"].ConvertTo<string>(),
                                FinalId = reader["FinalId"].ConvertTo<long>(),
                                InspectionId = reader["FinalId"].ConvertTo<long>(),
                                FinalStatus = (StatusType)reader["FinalStatus"].ConvertTo<int>(),
                                MainLabel = reader["MainLabel"].ConvertTo<string>(),
                                WorkmanshipAQL = (AQL)reader["AQL"].ConvertTo<int>(),
                                MeasurementAQL = (AQL)reader["AQL2"].ConvertTo<int>(),
                                MeasurementComment = reader["MeasurementComment"].ConvertTo<string>(),
                                MeasurementPackingAudit = reader["MeasurementPackingAudit"].ConvertTo<string>(),
                                MeasurementStatus = (StatusType)reader["MeasurementStatus"].ConvertTo<int>(),
                                PackingComment = reader["PackingComment"].ConvertTo<string>(),
                                PackingStatus = (StatusType)reader["PackingStatus"].ConvertTo<int>(),
                                QAAuditor = reader["QAAuditor"].ConvertTo<string>(),
                                QAComment = reader["QAComment"].ConvertTo<string>(),
                                QAManager = reader["QAManager"].ConvertTo<string>(),
                                Type = (DHUType)reader["Type"].ConvertTo<int>(),
                                WorkmanshipComment = reader["WorkmanshipComment"].ConvertTo<string>(),
                                WorkmanshipStatus = (StatusType)reader["WorkmanshipStatus"].ConvertTo<int>(),
                                CreatedUser = reader["UserName"].ConvertTo<string>(),
                                UpdatedUser = reader["UpdatedUser"].ConvertTo<string>(),
                                ActualInspectionDate = reader["ActualInspectionDate"].ConvertTo<DateTime>().ToString("MM/dd/yyyy HH:mm:ss.fff")
                            };

                            entity.Styles = Select(entity.AIGLPO, entity.FinalId);
                            result.Add(entity);
                        }
                    }
                }
            }

            return result;
        }

        public IList<FinalCustPOEntity> Select(string aiglPO, long FinalId)
        {
            using (var sp = new StoredProcedure("sp_Final_SelectAllPO"))
            {
                var result = new List<FinalCustPOEntity>();
                var param = new Hashtable();
                param.Add("aiglpo", aiglPO);
                param.Add("finalid", FinalId);

                sp.AssignParamValues(param);
                using (var reader = sp.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            result.Add(new FinalCustPOEntity
                            {
                                Color = reader["Color"].ConvertTo<string>(),
                                CustPO = reader["CustPO"].ConvertTo<string>(),
                                OrderQuantity = reader["Total"].ConvertTo<int>(),
                                Style = reader["Style"].ConvertTo<string>(),
                                AIGLPO = reader["AIGLPO"].ConvertTo<string>(),
                                InspectedQty = reader["InspectedQty"].ConvertTo<int>(),
                                ActualProductQty = reader["ActualProductQty"].ConvertTo<int>(),
                                FinalDetailId = reader["FinalDetailId"].ConvertTo<long>()
                            });
                        }
                    }
                }

                return result;
            }
        }

        public Result Insert(FinalEntity entity)
        {
            using (var sp = new StoredProcedure("sp_Final_Insert"))
            {
                var param = new Hashtable();
                param.Add("workmanshipstatus", (int)entity.WorkmanshipStatus);
                param.Add("workmanshipcomment", entity.WorkmanshipComment);
                param.Add("qamanager", entity.QAManager);
                param.Add("type", (int)entity.Type);
                param.Add("qacomment", entity.QAComment);
                param.Add("qaauditor", entity.QAAuditor);
                param.Add("packingstatus", (int)entity.PackingStatus);
                param.Add("packingcomment", entity.PackingComment);
                param.Add("measurementstatus", (int)entity.MeasurementStatus);
                param.Add("measurementpackingaudit", entity.MeasurementPackingAudit);
                param.Add("measurementcomment", entity.MeasurementComment);
                param.Add("mainlabel", entity.MainLabel);
                param.Add("finalstatus", (int)entity.FinalStatus);
                param.Add("finalcomment", entity.FinalComment);
                param.Add("factoryrep", entity.FactoryRep);
                param.Add("factorymanager", entity.FactoryManager);
                param.Add("factorycaption", entity.FactoryCaption);
                param.Add("custpo", entity.CustPO);
                param.Add("custid", entity.CustId);
                param.Add("createddate", entity.CreatedDate);
                param.Add("createdby", entity.CreatedBy);
                param.Add("comment", entity.Comment);
                param.Add("cartoncomment", entity.CartonComment);
                param.Add("auditedby", entity.AuditedBy);
                param.Add("aql2", (int)entity.MeasurementAQL);
                param.Add("aql", (int)entity.WorkmanshipAQL);
                param.Add("aiglpo", entity.AIGLPO);
                param.Add("finalid", entity.FinalId);
                param.Add("errmsg", string.Empty);
                param.Add("actualinspectiondate", entity.ActualInspectionDate);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                var id = sp.GetParameterValue("finalid").ConvertTo<int>();
                return new Result { Id = id, Message = id > 0 ? "Created successfully." : sp.GetParameterValue("errmsg").ConvertTo<string>() };
            }
        }

        public Result Update(FinalEntity entity)
        {
            using (var sp = new StoredProcedure("sp_Final_Update"))
            {
                var param = new Hashtable();
                param.Add("workmanshipstatus", (int)entity.WorkmanshipStatus);
                param.Add("workmanshipcomment", entity.WorkmanshipComment);
                param.Add("qamanager", entity.QAManager);
                param.Add("qacomment", entity.QAComment);
                param.Add("qaauditor", entity.QAAuditor);
                param.Add("packingstatus", (int)entity.PackingStatus);
                param.Add("packingcomment", entity.PackingComment);
                param.Add("measurementstatus", (int)entity.MeasurementStatus);
                param.Add("measurementpackingaudit", entity.MeasurementPackingAudit);
                param.Add("measurementcomment", entity.MeasurementComment);
                param.Add("mainlabel", entity.MainLabel);
                param.Add("finalstatus", (int)entity.FinalStatus);
                param.Add("finalcomment", entity.FinalComment);
                param.Add("factoryrep", entity.FactoryRep);
                param.Add("factorymanager", entity.FactoryManager);
                param.Add("factorycaption", entity.FactoryCaption);
                param.Add("comment", entity.Comment);
                param.Add("cartoncomment", entity.CartonComment);
                param.Add("auditedby", entity.AuditedBy);
                param.Add("aql2", (int)entity.MeasurementAQL);
                param.Add("aql", (int)entity.WorkmanshipAQL);
                param.Add("updatedby", entity.UpdatedBy);
                param.Add("updateddate", entity.UpdatedDate);
                param.Add("finalid", entity.FinalId);
                param.Add("actualinspectiondate", entity.ActualInspectionDate);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                return new Result { Id = entity.FinalId, Message = "Updated successfully." };
            }
        }

        public Result InsertDetail(FinalCustPOEntity entity)
        {
            using (var sp = new StoredProcedure("sp_FinalDetail_Insert"))
            {
                var param = new Hashtable();
                param.Add("finalid", entity.FinalId);
                param.Add("style", entity.Style);
                param.Add("color", entity.Color);
                param.Add("quantity", entity.OrderQuantity);
                param.Add("inspectedqty", entity.InspectedQty);
                param.Add("actualproductqty", entity.ActualProductQty);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                return new Result {  };
            }
        }

        public Result UpdateDetail(FinalCustPOEntity entity)
        {
            using (var sp = new StoredProcedure("sp_FinalDetail_Update"))
            {
                var param = new Hashtable();
                param.Add("finaldetailid", entity.FinalDetailId);
                param.Add("finalid", entity.FinalId);
                param.Add("style", entity.Style);
                param.Add("color", entity.Color);
                param.Add("quantity", entity.OrderQuantity);
                param.Add("inspectedqty", entity.InspectedQty);
                param.Add("actualproductqty", entity.ActualProductQty);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();

                return new Result { };
            }
        }

        public void Delete(long finalId, string img)
        {
            using (var sp = new StoredProcedure("sp_Final_DeleteImg"))
            {
                var param = new Hashtable();
                param.Add("img", img);
                param.Add("finalid", finalId);

                sp.AssignParamValues(param);
                sp.ExecuteNonQuery();
            }
        }

        #endregion
    }
}