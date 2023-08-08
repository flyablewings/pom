using System;
using System.Linq;
using POMQC.Data.Report;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Report;

namespace POMQC.Services.Report
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository = new ReportRepository();

        #region IReportService Members

        public ReportViewModel ReportByCustPO(long custId, string custPO, string aiglPO)
        {
            var data = _repository.ReportByCustPO(custId, custPO, aiglPO);
            var result = new ReportViewModel();
            result.ReportByPO = new ReportViewModelBase { Items = data };

            return result;
        }

        public ReportViewModel ReportByFactory(int factoryId, DateTime from, DateTime to)
        {
            var data = _repository.ReportByFactory(factoryId, from, to);
            var result = new ReportViewModel();
            result.ReportByFactory = new ReportViewModelBase { Items = data };

            return result;
        }

        public ReportViewModel ReportAllFactories(DateTime from, DateTime to)
        {
            var data = _repository.ReportAllFactories(from, to);
            var result = new ReportViewModel();
            result.ReportAllFactories = new ReportViewModelBaseFactory
            {
                Items = data,
                Factories = data.GroupBy(i => i.FactoryName),
                DHUs = data.GroupBy(i => i.DHUType)
            };

            return result;
        }

        public ReportViewModel ReportAllDetail(DateTime from, DateTime to)
        {
            var data = _repository.ReportAllDetail(from, to);
            var result = new ReportViewModel();
            result.ReportDetailFactories = new ReportViewModelBaseFactory
            {
                Items = data,
                Factories = data.GroupBy(i => i.FactoryName),
                DHUs = data.GroupBy(i => i.DHUType)
            };

            return result;
        }

        public ReportViewModel ReportWeekly(DateTime from, DateTime to)
        {
            var data = _repository.ReportDHUWeekly(from, to);
            var result = new ReportViewModel { ReportDHUWeekly = new DHUWeeklyReportViewModel { ReportDHUWeekly = data } };

            return result;
        }

        public ReportViewModel ReportMonthly(DateTime from, DateTime to)
        {
            var data = _repository.ReportDHUMonthly(from, to);
            var result = new ReportViewModel { ReportDHUMonthly = new DHUMonthlyReportViewModel { ReportDHUMonthly = data } };

            return result;
        }

        #endregion
    }
}