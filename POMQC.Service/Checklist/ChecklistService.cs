using System;
using System.Collections.Generic;
using System.Linq;
using POMQC.Data.Checklist;
using POMQC.Entities.Base;
using POMQC.Entities.Checklist;
using POMQC.Utilities;
using POMQC.ViewModels.Checklist;

namespace POMQC.Services.Checklist
{
    public class ChecklistService : IChecklistService
    {
        private readonly IChecklistRepository _repository = new ChecklistRepository();

        #region IChecklistService Members

        public Result Insert(ChecklistViewModel model)
        {
            var entity = new ChecklistEntity
            {
                AIGLPO = model.AIGLPO,
                Comment = model.Comment,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                CustId = model.CustId,
                CustPO = model.CustPO,
                Doc = model.Doc,
                Type = (ChecklistType)model.Function
            };

            return _repository.Insert(entity);
        }

        public Result Update(ChecklistViewModel model)
        {
            var entity = new ChecklistEntity
            {
                AIGLPO = model.AIGLPO,
                Comment = model.Comment,
                UpdatedBy = model.UpdatedBy,
                UpdatedDate = model.UpdatedDate,
                CustId = model.CustId,
                CustPO = model.CustPO,
                Doc = model.Doc,
                Type = (ChecklistType)model.Function
            };

            return _repository.Update(entity);
        }

        public IList<ChecklistViewModel> Select(long custId, string custPO, string aiglPO, int type = 0)
        {
            var result = new List<ChecklistViewModel>();
            var data = _repository.Select(custId, custPO, aiglPO, type);

            foreach (var item in data)
            {                
                var checklist = new ChecklistViewModel
                {
                    Doc = item.Doc,
                    Comment = item.Comment,
                    Function = (ChecklistFunctionViewModel)item.Type,
                    Images = new List<string>(),
                    Documents = new List<string>(),
                    CustId = item.CustId,
                    CustPO = item.CustPO,
                    AIGLPO = item.AIGLPO,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    UpdatedBy = item.UpdatedBy,
                    UpdatedDate = item.UpdatedDate,
                    CreatedUser = item.CreatedUser,
                    UpdatedUser = item.UpdatedUser,
                    CreateDate = item.CreatedDate.ToString("MM/dd/yyyy HH:mm:ss.fff"),
                    UpdateDate = item.UpdatedDate.ToString("MM/dd/yyyy HH:mm:ss.fff")
                };

                var docs = item.Doc.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var doc in docs)
                {
                    if (Utils.ImageFileTypes.Split(',').Any(s => s.IndexOf(doc.Split('.').Last(), StringComparison.OrdinalIgnoreCase) != -1))
                    {
                        checklist.Images.Add(doc);
                    }

                    if (Utils.DocumentFileTypes.Split(',').Any(s => s.IndexOf(doc.Split('.').Last(), StringComparison.OrdinalIgnoreCase) != -1))
                    {
                        checklist.Documents.Add(doc);
                    }                    
                }

                checklist.DroppedFiles = item.DroppedFiles;
                result.Add(checklist);
            }

            return result;
        }

        public void Delete(long custId, string custPO, string aiglPO, string doc, int type)
        {
            _repository.Delete(custId, custPO, aiglPO, doc, type);
        }

        #endregion
    }
}