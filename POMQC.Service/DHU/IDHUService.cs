using System;
using System.Collections.Generic;
using POMQC.ViewModels.Base;

namespace POMQC.Services.DHU
{
    public interface IDHUService
    {
        IList<DHUViewModel> Select(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0);
    }
}
