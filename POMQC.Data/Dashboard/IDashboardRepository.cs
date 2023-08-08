using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POMQC.Entities;

namespace POMQC.Data.Dashboard
{
    public interface IDashboardRepository
    {
        IList<POInfoEntity> POs(DateTime from, DateTime to, long custId = 0, string custPO = "", string aiglPO = "");
    }
}
