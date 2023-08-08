using System;
using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.Entities.Defect;

namespace POMQC.Data.Report
{
    public interface IReportRepository
    {
        IList<DefectDetail> ReportByCustPO(long custId, string custPO, string aiglPO);

        IList<DefectDetail> ReportByFactory(int factoryId, DateTime from, DateTime to);

        IList<DefectDetail> ReportAllFactories(DateTime from, DateTime to);

        IList<DefectDetail> ReportAllDetail(DateTime from, DateTime to);

        IList<WeeklyDHUReportEntity> ReportDHUWeekly(DateTime from, DateTime to);

        IList<MonthlyDHUReportEntity> ReportDHUMonthly(DateTime from, DateTime to);
    }
}