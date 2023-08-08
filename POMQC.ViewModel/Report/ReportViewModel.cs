using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Filter;

namespace POMQC.ViewModels.Report
{
    public class ReportViewModel
    {
        public ReportViewModel()
        {
            ReportByPO = new ReportViewModelBase();
            ReportByFactory = new ReportViewModelBase();
            ReportAllFactories = new ReportViewModelBaseFactory();
            ReportDetailFactories = new ReportViewModelBaseFactory();
            ReportFinalViewModel = new ReportFinalViewModel();
            ReportDHUWeekly = new DHUWeeklyReportViewModel();
            ReportDHUMonthly = new DHUMonthlyReportViewModel();
            Factories = new List<FactoryBase>();
            Filter = new FilterViewModel();
        }

        public FilterViewModel Filter { get; set; }

        public ReportViewModelBase ReportByPO { get; set; }

        public ReportViewModelBase ReportByFactory { get; set; }

        public ReportViewModelBaseFactory ReportAllFactories { get; set; }

        public ReportViewModelBaseFactory ReportDetailFactories { get; set; }

        public ReportFinalViewModel ReportFinalViewModel { get; set; }

        public DHUWeeklyReportViewModel ReportDHUWeekly { get; set; }

        public DHUMonthlyReportViewModel ReportDHUMonthly { get; set; }

        public EditorModel DateRange { get; set; }

        public IList<FactoryBase> Factories { get; set; }
    }
}