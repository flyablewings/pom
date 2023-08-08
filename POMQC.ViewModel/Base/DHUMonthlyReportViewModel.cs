using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.ViewModels.Filter;

namespace POMQC.ViewModels.Base
{
    public class DHUMonthlyReportViewModel
    {
        public FunctionViewModel Function { get; set; }

        public string Title { get; set; }

        public IList<MonthlyDHUReportEntity> ReportDHUMonthly { get; set; }
    }
}