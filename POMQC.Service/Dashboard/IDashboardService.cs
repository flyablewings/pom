using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POMQC.ViewModels.Dashboard;

namespace POMQC.Services.Dashboard
{
    public interface IDashboardService
    {
        IList<POInfoViewModel> POs(DateTime from, DateTime to, long custId = 0, string custPO = "", string aiglPO = "", string status = "");
    }
}
