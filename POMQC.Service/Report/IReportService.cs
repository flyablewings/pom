using System;
using POMQC.ViewModels.Report;

namespace POMQC.Services.Report
{
    public interface IReportService
    {
        ReportViewModel ReportByCustPO(long custId, string custPO, string aiglPO);

        ReportViewModel ReportByFactory(int factoryId, DateTime from, DateTime to);

        ReportViewModel ReportAllFactories(DateTime from, DateTime to);

        ReportViewModel ReportAllDetail(DateTime from, DateTime to);

        ReportViewModel ReportWeekly(DateTime from, DateTime to);

        ReportViewModel ReportMonthly(DateTime from, DateTime to);
    }
}