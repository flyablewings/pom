using System;
using System.Collections.Generic;
using POMQC.Entities.Final;
using POMQC.ViewModels.Base;

namespace POMQC.Services.Final
{
    public interface IFinalService
    {
        IList<FinalViewModel> Select(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0);

        IList<FinalCustPOEntity> Select(string aiglPO, long FinalId);

        IList<FinalEntity> SelectAll(DateTime from, DateTime to);
    }
}
