using System;
using System.Linq;
using System.Web.Mvc;
using POMQC.Controllers.Account;
using POMQC.Services.Report;
using POMQC.Services.Final;
using POMQC.ViewModels.Base;

namespace POMQC.Controllers.Report
{
    public class ReportController : BaseController
    {
        private readonly IReportService _report = new ReportService();
        private readonly IFinalService _final = new FinalService();

        [AuroraAuthorize]
        public ActionResult GetByCustPO(long custId, string custPO, string aiglPO, string custName)
        {
            var result = _report.ReportByCustPO(custId, custPO, aiglPO);
            var report = result.ReportByPO.Items.GroupBy(i => i.DHUType);
            var title = string.Format("DEFECTIVE REPORT BY PO<br />Customer: {0}, Customer PO: {1}, AIGL PO: {2}", custName, custPO, aiglPO);

            return JsonBase(new { Title = title, Items = report });
        }

        [AuroraAuthorize]
        public ActionResult GetByFactory(int factoryId, string from, string to, string factoryName)
        {
            var result = _report.ReportByFactory(factoryId, Convert.ToDateTime(from), Convert.ToDateTime(to));
            var report = result.ReportByFactory.Items.GroupBy(i => i.DHUType);
            var title = string.Format("DEFECTIVE REPORT BY FACTORY<br />Factory: {0}, From: {1}, To: {2}", factoryName, from, to);

            return JsonBase(new { Title = title, Items = report });
        }

        [AuroraAuthorize]
        public ActionResult GetAllFactories(string from, string to, string factoryName)
        {
            var result = _report.ReportAllFactories(Convert.ToDateTime(from), Convert.ToDateTime(to));
            var title = string.Format("DEFECTIVE REPORT FOR ALL FACTORIES<br />From: {0}, To: {1}", from, to);

            return JsonBase(new { Title = title, Items = result.ReportAllFactories.Items, Factories = result.ReportAllFactories.Factories, DHUs = result.ReportAllFactories.DHUs });
        }

        [AuroraAuthorize]
        public ActionResult GetByCustPoExcel(long custId, string custPO, string aiglPO, string custName)
        {
            var result = _report.ReportByCustPO(custId, custPO, aiglPO);
            var title = string.Format("DEFECTIVE REPORT BY PO<br />Customer: {0}, Customer PO: {1}, AIGL PO: {2}", custName, custPO, aiglPO);
            var model = result.ReportByPO;
            model.Title = title;
            var excel = RenderRazorViewToString("_ReportExcelCustOrFact", model);

            return ExportExcel(excel, string.Format("CUSTOMER-REPORT-{0}-{1}-{2}", custName, custPO, aiglPO));
        }

        [AuroraAuthorize]
        public ActionResult GetByFactoryExcel(int factoryId, string from, string to, string factoryName)
        {
            var result = _report.ReportByFactory(factoryId, Convert.ToDateTime(from), Convert.ToDateTime(to));
            var title = string.Format("DEFECTIVE REPORT BY FACTORY<br />Factory: {0}, From: {1}, To: {2}", factoryName, from, to);
            var model = result.ReportByFactory;
            model.Title = title;
            var excel = RenderRazorViewToString("_ReportExcelCustOrFact", model);

            return ExportExcel(excel, string.Format("FACTORY-REPORT-{0}-{1}-{2}", factoryName, from.Replace('/', '_'), to.Replace('/', '_')));
        }

        [AuroraAuthorize]
        public ActionResult GetAllFactoriesExcel(string from, string to, string factoryName)
        {
            var result = _report.ReportAllFactories(Convert.ToDateTime(from), Convert.ToDateTime(to));
            var title = string.Format("DEFECTIVE REPORT FOR ALL FACTORIES<br />From: {0}, To: {1}", from, to);
            var model = result.ReportAllFactories;
            model.Title = title;
            var excel = RenderRazorViewToString("_ReportExcelAllFact", model);

            return ExportExcel(excel, string.Format("ALL-FACTORIES-REPORT-{0}-{1}-{2}", factoryName, from.Replace('/', '_'), to.Replace('/', '_')));
        }

        [AuroraAuthorize]
        public ActionResult GetAllFinal(string from, string to)
        {
            var result = _final.SelectAll(Convert.ToDateTime(from), Convert.ToDateTime(to));
            var title = string.Format("REPORT FOR PREFINAL AND FINAL<br />From: {0}, To: {1}", from, to);

            return JsonBase(new ReportFinalViewModel { Title = title, Items = result });
        }

        [AuroraAuthorize]
        public ActionResult GetAllFinalExcel(string from, string to)
        {
            var result = _final.SelectAll(Convert.ToDateTime(from), Convert.ToDateTime(to));
            var title = string.Format("REPORT FOR PREFINAL AND FINAL<br />From: {0}, To: {1}", from, to);

            var model = new ReportFinalViewModel { Items = result, Title = title };
            var excel = RenderRazorViewToString("_ReportExcelFinal", model);

            return ExportExcel(excel, string.Format("PREFINAL-FINAL-REPORT-{0}-{1}", from.Replace('/', '_'), to.Replace('/', '_')));
        }

        [AuroraAuthorize]
        public ActionResult GetAllDetail(string from, string to)
        {
            var result = _report.ReportAllDetail(Convert.ToDateTime(from), Convert.ToDateTime(to));
            var title = string.Format("DETAIL REPORT<br />From: {0}, To: {1}", from, to);

            return JsonBase(new { Title = title, Items = result.ReportDetailFactories.Items.GroupBy(m => new { m.FactoryName, m.CustName, m.CustPO, m.AIGLPO, m.DHUType, m.CreatedDate, m.CreatedUser }) });
        }

        [AuroraAuthorize]
        public ActionResult GetAllDetailExcel(string from, string to)
        {
            var result = _report.ReportAllDetail(Convert.ToDateTime(from), Convert.ToDateTime(to));
            var title = string.Format("DETAIL REPORT<br />From: {0}, To: {1}", from, to);

            var model = result.ReportDetailFactories;
            model.Title = title;
            var excel = RenderRazorViewToString("_ReportExcelDetail", model);

            return ExportExcel(excel, string.Format("DETAIL-REPORT-{0}-{1}", from.Replace('/', '_'), to.Replace('/', '_')));
        }

        [AuroraAuthorize]
        public ActionResult GetDHUWeekly(string from, string to)
        {
            var result = _report.ReportWeekly(Convert.ToDateTime(from), Convert.ToDateTime(to));
            var title = string.Format("DHU WEEKLY REPORT<br />From: {0}, To: {1}", from, to);

            return JsonBase(new { Title = title, Items = result.ReportDHUWeekly.ReportDHUWeekly });
        }

        [AuroraAuthorize]
        public ActionResult GetDHUWeeklyExcel(string from, string to)
        {
            var result = _report.ReportWeekly(Convert.ToDateTime(from), Convert.ToDateTime(to));
            var title = string.Format("DHU WEEKLY REPORT<br />From: {0}, To: {1}", from, to);

            var model = result.ReportDHUWeekly;
            model.Title = title;
            var excel = RenderRazorViewToString("_ReportExcelDHUWeekly", model);

            return ExportExcel(excel, string.Format("DHU-WEEKLY-REPORT-{0}-{1}", from.Replace('/', '_'), to.Replace('/', '_')));
        }

        [AuroraAuthorize]
        public ActionResult GetDHUMonthly(string from, string to)
        {
            var result = _report.ReportMonthly(Convert.ToDateTime(from), Convert.ToDateTime(to));
            var title = string.Format("DHU MONTHLY REPORT<br />From: {0}, To: {1}", from, to);

            return JsonBase(new { Title = title, Items = result.ReportDHUMonthly.ReportDHUMonthly });
        }

        [AuroraAuthorize]
        public ActionResult GetDHUMonthlyExcel(string from, string to)
        {
            var result = _report.ReportMonthly(Convert.ToDateTime(from), Convert.ToDateTime(to));
            var title = string.Format("DHU MONTHLY REPORT<br />From: {0}, To: {1}", from, to);

            var model = result.ReportDHUMonthly;
            model.Title = title;
            var excel = RenderRazorViewToString("_ReportExcelDHUMonthly", model);

            return ExportExcel(excel, string.Format("DHU-MONTHLY-REPORT-{0}-{1}", from.Replace('/', '_'), to.Replace('/', '_')));
        }
    }
}