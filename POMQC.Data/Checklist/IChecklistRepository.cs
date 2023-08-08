using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.Entities.Checklist;

namespace POMQC.Data.Checklist
{
    public interface IChecklistRepository
    {
        Result Insert(ChecklistEntity entity);

        Result Update(ChecklistEntity entity);

        void Delete(long custId, string custPO, string aiglPO, string doc, int type);

        IList<ChecklistEntity> Select(long custId, string custPO, string aiglPO, int type = 0);
    }
}
