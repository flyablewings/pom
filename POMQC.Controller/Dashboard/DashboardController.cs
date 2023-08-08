using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using POMQC.Entities.Final;
using POMQC.Services.Checklist;
using POMQC.Services.Dashboard;
using POMQC.Services.Inspection;
using POMQC.Services.Report;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Checklist;
using POMQC.ViewModels.Dashboard;
using POMQC.ViewModels.Filter;
using POMQC.ViewModels.Inspection;
using POMQC.ViewModels.Report;
using POMQC.Services.Defect;
using POMQC.Entities.Defect;

namespace POMQC.Controllers.Dashboard
{
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboard = new DashboardService();
        private readonly IChecklistService _checklist = new ChecklistService();
        private readonly IInspectionService _inspection = new InspectionService();
        private readonly IReportService _report = new ReportService();
        private readonly IDefectService _defect = new DefectService();

        public JsonResult View(long custId, string custPO, string aiglPO, DateTime from, DateTime to, string custName, string status, int factoryId)
        {
            var model = new DashboardViewModel();
            var poss = _dashboard.POs(from, to, custId, custPO, aiglPO, status);
            model.POs = factoryId == 0 ? poss : poss.Where(p => p.FactoryId == factoryId).ToList();

            var firstPO = model.POs.FirstOrDefault();
            IList<ChecklistViewModel> checklist = new List<ChecklistViewModel>();
            IList<DHUViewModel> inspections = new List<DHUViewModel>();
            IList<FinalViewModel> finals = new List<FinalViewModel>();
            IList<ReportViewModelBase> reports = new List<ReportViewModelBase>();
            var inspection = new InspectionViewModel();
            var pos = new List<FinalCustPOEntity>();
            var report = new ReportViewModel();

            if (firstPO != null)
            {
                checklist = _checklist.Select(firstPO.CustId, firstPO.CustPO, firstPO.AIGLPO);
                inspection = _inspection.Select(firstPO.CustId, firstPO.CustPO, firstPO.AIGLPO);
                report = _report.ReportByCustPO(firstPO.CustId, firstPO.CustPO, firstPO.AIGLPO);
                custName = string.IsNullOrWhiteSpace(custName) || custName == "All" ? firstPO.Customer : custName;
                custPO = string.IsNullOrWhiteSpace(custPO) || custPO == "All" ? firstPO.CustPO : custPO;
                aiglPO = string.IsNullOrWhiteSpace(aiglPO) || aiglPO == "All" ? firstPO.AIGLPO : aiglPO;
            }

            model.FitSample = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.FitSample, "FIT SAMPLE APPROVED", true);
            model.PPSample = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.PPSample, "PP SAMPLE APPROVED", false);
            model.TopSample = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.TopSample, "TOP SAMPLE APPROVED", false);
            model.QAChecklist = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.QAChecklist, "QA/QC CHECKLIST", false);
            model.PPMeeting = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.PPMeeting, "PP MEETING", false);
            model.Fabric = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.Fabric, "FABRIC INSPECTION", false);

            // Set Inspection
            model.Cutting = inspection.Cutting;
            model.Inline = inspection.Inline;
            model.Endline = inspection.Endline;
            model.Finishing = inspection.Finishing;
            model.Packing = inspection.Packing;
            model.Prefinal = inspection.Prefinal;
            model.Final = inspection.Final;
                        
            report.ReportByPO.Function = FunctionViewModel.ReportByPO;
            report.ReportByPO.Title = "SUMMARY DEFECTIVE INSPECTION BY PO";
            model.Summary = report.ReportByPO;
            model.ReportItems = report.ReportByPO.Items.GroupBy(i => i.DHUType);
            model.ReportTitle = string.Format("SUMMARY DEFECTIVE INSPECTION BY PO<br />Customer: {0}, Customer PO: {1}, AIGL PO: {2}", custName, custPO, aiglPO);
            model.Styles = inspection.Styles;

            return JsonBase(model);
        }

        public ActionResult ViewDetail(long custId, string custPO, string aiglPO, DateTime from, DateTime to, string custName, string status, int factoryId)
        {
            var model = new DashboardViewModel();
            var pos = _dashboard.POs(from, to, custId, custPO, aiglPO, status);
            model.POs = factoryId == 0 ? pos : pos.Where(p => p.FactoryId == factoryId).ToList();
            
            IList<ChecklistViewModel> checklist = new List<ChecklistViewModel>();
            IList<DHUViewModel> inspections = new List<DHUViewModel>();
            IList<FinalViewModel> finals = new List<FinalViewModel>();
            IList<ReportViewModelBase> reports = new List<ReportViewModelBase>();

            checklist = _checklist.Select(custId, custPO, aiglPO);

            model.FitSample = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.FitSample, "FIT SAMPLE APPROVED", true);
            model.PPSample = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.PPSample, "PP SAMPLE APPROVED", false);
            model.TopSample = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.TopSample, "TOP SAMPLE APPROVED", false);
            model.QAChecklist = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.QAChecklist, "QA/QC CHECKLIST", false);
            model.PPMeeting = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.PPMeeting, "PP MEETING", false);
            model.Fabric = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.Fabric, "FABRIC INSPECTION", false);

            // Query Inspection
            var inspection = _inspection.Select(custId, custPO, aiglPO);

            // Set Inspection
            model.Cutting = inspection.Cutting;
            model.Inline = inspection.Inline;
            model.Endline = inspection.Endline;
            model.Finishing = inspection.Finishing;
            model.Packing = inspection.Packing;
            model.Prefinal = inspection.Prefinal;
            model.Final = inspection.Final;

            // Set report
            var report = _report.ReportByCustPO(custId, custPO, aiglPO);
            report.ReportByPO.Function = FunctionViewModel.ReportByPO;
            report.ReportByPO.Title = "SUMMARY DEFECTIVE INSPECTION BY PO";
            model.Summary = report.ReportByPO;
            model.ReportItems = report.ReportByPO.Items.GroupBy(i => i.DHUType);
            model.ReportTitle = string.Format("SUMMARY DEFECTIVE INSPECTION BY PO<br />Customer: {0}, Customer PO: {1}, AIGL PO: {2}", custName, custPO, aiglPO);
            model.Styles = inspection.Styles;

            return JsonBase(model);
        }
    }
}
