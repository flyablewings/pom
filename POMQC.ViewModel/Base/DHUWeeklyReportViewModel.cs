using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.ViewModels.Filter;

namespace POMQC.ViewModels.Base
{
    public class DHUWeeklyReportViewModel
    {
        public FunctionViewModel Function { get; set; }

        public string Title { get; set; }

        public IList<WeeklyDHUReportEntity> ReportDHUWeekly { get; set; }
    }
}