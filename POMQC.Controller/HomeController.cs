using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using POMQC.Controllers.Account;
using POMQC.Entities.Defect;
using POMQC.Services.Checklist;
using POMQC.Services.Dashboard;
using POMQC.Services.Defect;
using POMQC.Services.DHU;
using POMQC.Services.Filter;
using POMQC.Services.Inspection;
using POMQC.Services.Report;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Checklist;
using POMQC.ViewModels.Dashboard;
using POMQC.ViewModels.Defect;
using POMQC.ViewModels.Filter;
using POMQC.ViewModels.Inspection;
using POMQC.ViewModels.Report;
using POMQC.Services.Final;

namespace POMQC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IChecklistService _checklist = new ChecklistService();
        private readonly IFilterService _filter = new FilterService();
        private readonly IDashboardService _dashboard = new DashboardService();
        private readonly IDHUService _dhu = new DHUService();
        private readonly IInspectionService _inspection = new InspectionService();
        private readonly IDefectService _defect = new DefectService();
        private readonly IReportService _report = new ReportService();
        private readonly IFinalService _final = new FinalService();
                         
        [AuroraAuthorize]
        public ActionResult Dashboard()
        {
            var model = new DashboardViewModel();
            var inspection = new InspectionViewModel();
            model.Filter = _filter.Filters(true, false, true, FunctionViewModel.Dashboard, true);
            model.POs = _dashboard.POs(Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString()),
                Convert.ToDateTime(DateTime.Now.AddMonths(1).ToShortDateString()), 0, string.Empty, string.Empty, "Opening");

            var firstPO = model.POs.FirstOrDefault();
            IList<ChecklistViewModel> checklist = new List<ChecklistViewModel>();
            IList<FinalViewModel> finals = new List<FinalViewModel>();
            IList<ReportViewModelBase> reports = new List<ReportViewModelBase>();
            var report = new ReportViewModel();

            if (firstPO != null)
            {
                checklist = _checklist.Select(firstPO.CustId, firstPO.CustPO, firstPO.AIGLPO);
                inspection = _inspection.Select(firstPO.CustId, firstPO.CustPO, firstPO.AIGLPO);
                report = _report.ReportByCustPO(firstPO.CustId, firstPO.CustPO, firstPO.AIGLPO);
            }

            var codes = _defect.DefectCodes();
            var locs = _defect.DefectionLocations();

            codes.Insert(0, new DefectCode { DefName = "None", Active = true });
            locs.Insert(0, new DefectLocation { LocName = "None", Active = true });

            inspection.Cutting.DefectCodes = codes;
            inspection.Cutting.DefectLocations = locs;

            // Checklist
            model.FitSample = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.FitSample, "FIT SAMPLE APPROVED", true);
            model.PPSample = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.PPSample, "PP SAMPLE APPROVED", false);
            model.TopSample = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.TopSample, "TOP SAMPLE APPROVED", false);
            model.QAChecklist = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.QAChecklist, "QA/QC CHECKLIST", false);
            model.PPMeeting = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.PPMeeting, "PP MEETING", false);
            model.Fabric = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.Fabric, "FABRIC INSPECTION", false);

            // Set Inspection
            inspection.Cutting.IsReadOnly = true;
            inspection.Inline.IsReadOnly = true;
            inspection.Endline.IsReadOnly = true;
            inspection.Finishing.IsReadOnly = true;
            inspection.Packing.IsReadOnly = true;
            inspection.Prefinal.IsReadOnly = true;
            inspection.Final.IsReadOnly = true;

            model.Cutting = inspection.Cutting;
            model.Inline = inspection.Inline;
            model.Endline = inspection.Endline;
            model.Finishing = inspection.Finishing;
            model.Packing = inspection.Packing;
            model.Prefinal = inspection.Prefinal;
            model.Final = inspection.Final;

            // Set report
            var reportTitle = "SUMMARY DEFECTIVE INSPECTION BY PO";
            var reportCustInfo = "<br />Customer: {0}, Customer PO: {1}, AIGL PO: {2}";
            reportTitle +=
                firstPO != null ? string.Format(reportCustInfo, firstPO.Customer, firstPO.CustPO, firstPO.AIGLPO) : string.Empty;
            report.ReportByPO.Function = FunctionViewModel.ReportByPO;
            report.ReportByPO.Title = reportTitle;
            model.Summary = report.ReportByPO;

            return View(model);
        }

        [AuroraAuthorize]
        public ActionResult Sample()
        {
            var filter = _filter.Filters(true, true, true, FunctionViewModel.Sample, true);
            var model = new ChecklistViewModel();
            model.Filter = filter;
            model.POs = _dashboard.POs(Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString()), Convert.ToDateTime(DateTime.Now.AddMonths(1).ToShortDateString()), 0, string.Empty, string.Empty, "Opening");

            var firstPO = model.POs.FirstOrDefault();
            if (firstPO != null)
            {
                var custId = firstPO.CustId;
                var custPO = firstPO.CustPO;
                var aiglPO = firstPO.AIGLPO;                
                model.Items = _checklist.Select(custId, custPO, aiglPO).Where(i => i.Function == ChecklistFunctionViewModel.FitSample ||
                    i.Function == ChecklistFunctionViewModel.PPSample ||
                    i.Function == ChecklistFunctionViewModel.TopSample).ToList();
            }

            return View(model ?? new ChecklistViewModel());
        }

        [AuroraAuthorize]
        public ActionResult Quality()
        {
            var filter = _filter.Filters(true, true, true, FunctionViewModel.Checklist, true);
            var model = new ChecklistViewModel();
            model.Filter = filter;
            model.POs = _dashboard.POs(Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString()),
                Convert.ToDateTime(DateTime.Now.AddMonths(1).ToShortDateString()), 0, string.Empty, string.Empty, "Opening");

            var firstPO = model.POs.FirstOrDefault();
            if (firstPO != null)
            {
                var custId = firstPO.CustId;
                var custPO = firstPO.CustPO;
                var aiglPO = firstPO.AIGLPO;
                model.Items = _checklist.Select(custId, custPO, aiglPO).Where(i => i.Function == ChecklistFunctionViewModel.QAChecklist).ToList();
            }

            return View(model ?? new ChecklistViewModel());
        }

        [AuroraAuthorize]
        public ActionResult Meeting()
        {
            var filter = _filter.Filters(true, true, true, FunctionViewModel.Meeting, true);
            var model = new ChecklistViewModel();
            model.Filter = filter;
            model.POs = _dashboard.POs(Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString()),
                Convert.ToDateTime(DateTime.Now.AddMonths(1).ToShortDateString()), 0, string.Empty, string.Empty, "Opening");

            var firstPO = model.POs.FirstOrDefault();
            if (firstPO != null)
            {
                var custId = firstPO.CustId;
                var custPO = firstPO.CustPO;
                var aiglPO = firstPO.AIGLPO;
                model.Items = _checklist.Select(custId, custPO, aiglPO).Where(i => i.Function == ChecklistFunctionViewModel.PPMeeting).ToList();
            }

            return View(model ?? new ChecklistViewModel());
        }

        [AuroraAuthorize]
        public ActionResult Inspection()
        {
            var filter = _filter.Filters(true, true, true, FunctionViewModel.Inspection, true);            
            var model = new InspectionViewModel();
            model.Filter = filter;
            model.POs = _dashboard.POs(Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString()),
                Convert.ToDateTime(DateTime.Now.AddMonths(1).ToShortDateString()), 0, string.Empty, string.Empty, "Opening");

            var firstPO = model.POs.FirstOrDefault();
            if (firstPO != null)
            {
                var custId = firstPO.CustId;
                var custPO = firstPO.CustPO;
                var aiglPO = firstPO.AIGLPO;
                model = _inspection.Select(custId, custPO, aiglPO);
                model.POs = _dashboard.POs(Convert.ToDateTime(DateTime.Now.AddMonths(-1).ToShortDateString()), 
                    Convert.ToDateTime(DateTime.Now.AddMonths(1).ToShortDateString()));
            }

            var codes = _defect.DefectCodes();
            var locs = _defect.DefectionLocations();

            codes.Insert(0, new DefectCode { DefName = "None", Active = true });
            locs.Insert(0, new DefectLocation { LocName = "None", Active = true });

            model.Cutting.DefectCodes = codes;
            model.Cutting.DefectLocations = locs;
            model.Fabric.IsActiveTab = true;

            return View(model);
        }

        [AuroraAuthorize]
        public ActionResult Report()
        {
            var model = new ReportViewModel
            {
                Filter = _filter.Filters(false, false, true, FunctionViewModel.ReportByPO, false, false, false),
                DateRange = new EditorModel(),
                Factories = _filter.Factories()
            };

            var custId = model.Filter.Customers.FirstOrDefault().CustId;
            var custPO = model.Filter.CustPOs.FirstOrDefault(c => c.Parent == custId.ToString()).PO;
            var aiglPO = model.Filter.AIGLPOs.FirstOrDefault(c => c.Parent == custPO).PO;
            var reportByPOViewModel = _report.ReportByCustPO(custId, custPO, aiglPO);
            var from = DateTime.Now.AddMonths(-1).ToString("MM/dd/yyyy");
            var to = DateTime.Now.AddMonths(1).ToString("MM/dd/yyyy");
            var reportByFactoryViewModel = _report.ReportByFactory(model.Factories.FirstOrDefault().FactoryId, Convert.ToDateTime(from), Convert.ToDateTime(to));
            var reportAllFactoriesViewModel = _report.ReportAllFactories(Convert.ToDateTime(from), Convert.ToDateTime(to));

            var reportTitlePO = "DEFECTIVE REPORT BY PO";
            var reportCustInfo = "<br />Customer: {0}, Customer PO: {1}, AIGL PO: {2}";
            reportTitlePO += string.Format(reportCustInfo, model.Filter.Customers.FirstOrDefault().CustName, custPO, aiglPO);

            var reportTitleFactory = "DEFECTIVE REPORT BY FACTORY";
            var reportFactInfo = "<br />Factory: {0}, From: {1}, To: {2}";
            reportTitleFactory += string.Format(reportFactInfo, model.Factories.FirstOrDefault().FactoryName, from, to);

            var reportTitleAllFactories = "DEFECTIVE REPORT FOR ALL FACTORIES";
            var reportAllFactInfo = "<br />From: {0}, To: {1}";
            reportTitleAllFactories += string.Format(reportAllFactInfo, from, to);

            var reportPrefinalFinal = _final.SelectAll(Convert.ToDateTime(from), Convert.ToDateTime(to));
            var reportPrefinalFinalTitle = string.Format("REPORT FOR PREFINAL AND FINAL<br />From: {0}, To: {1}", from, to);

            var reportFinalViewModel = new ReportFinalViewModel();
            reportFinalViewModel.Title = reportPrefinalFinalTitle;
            reportFinalViewModel.Items = reportPrefinalFinal;
            reportFinalViewModel.Function = FunctionViewModel.ReportFinal;

            var reportDetail = _report.ReportAllDetail(Convert.ToDateTime(from), Convert.ToDateTime(to));
            reportDetail.ReportDetailFactories.Title = string.Format("DETAIL REPORT<br />From: {0}, To: {1}", from, to);
            reportDetail.ReportDetailFactories.Function = FunctionViewModel.ReportDhuDetail;

            var reportDHUWeekly = _report.ReportWeekly(Convert.ToDateTime(from), Convert.ToDateTime(to));
            reportDHUWeekly.ReportDHUWeekly.Title = string.Format("DHU WEEKLY REPORT<br />From: {0}, To: {1}", from, to);
            reportDHUWeekly.ReportDHUWeekly.Function = FunctionViewModel.ReportDhuWeekly;

            var reportDHUMonthly = _report.ReportMonthly(Convert.ToDateTime(from), Convert.ToDateTime(to));
            reportDHUMonthly.ReportDHUMonthly.Title = string.Format("DHU MONTHLY REPORT<br />From: {0}, To: {1}", from, to);
            reportDHUMonthly.ReportDHUMonthly.Function = FunctionViewModel.ReportDhuMonthly;

            reportByPOViewModel.ReportByPO.Function = FunctionViewModel.ReportByPO;
            reportByPOViewModel.ReportByPO.Title = reportTitlePO;

            reportByFactoryViewModel.ReportByFactory.Function = FunctionViewModel.ReportByFactory;
            reportByFactoryViewModel.ReportByFactory.Title = reportTitleFactory;

            reportAllFactoriesViewModel.ReportAllFactories.Function = FunctionViewModel.ReportAllFactories;
            reportAllFactoriesViewModel.ReportAllFactories.Title = reportTitleAllFactories;

            model.ReportByPO = reportByPOViewModel.ReportByPO;
            model.ReportByFactory = reportByFactoryViewModel.ReportByFactory;
            model.ReportAllFactories = reportAllFactoriesViewModel.ReportAllFactories;
            model.ReportFinalViewModel = reportFinalViewModel;
            model.ReportDetailFactories = reportDetail.ReportDetailFactories;
            model.ReportDHUWeekly = reportDHUWeekly.ReportDHUWeekly;
            model.ReportDHUMonthly = reportDHUMonthly.ReportDHUMonthly;

            return View(model);
        }

        [AuroraAuthorize]
        public ActionResult Defect()
        {
            var defectCategories = _defect.DefectCategories();
            var defectCodes = _defect.DefectCodes();
            var defectLocations = _defect.DefectionLocations();

            return View(new DefectViewModel { Categories = defectCategories, Codes = defectCodes, Locations = defectLocations });
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("~/Account/Login");
        }
    }
}