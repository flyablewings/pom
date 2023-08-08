using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.ViewModels.Checklist;

namespace POMQC.Services.Checklist
{
    public interface IChecklistService
    {
        Result Insert(ChecklistViewModel model);

        Result Update(ChecklistViewModel model);

        void Delete(long custId, string custPO, string aiglPO, string doc, int type);
        
        IList<ChecklistViewModel> Select(long custId, string custPO, string aiglPO, int type = 0);
    }
}
