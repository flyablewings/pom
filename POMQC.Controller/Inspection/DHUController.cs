using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using POMQC.Controllers.Account;
using POMQC.Entities.Base;
using POMQC.Entities.DHU;
using POMQC.Services.Checklist;
using POMQC.Services.Dashboard;
using POMQC.Services.Defect;
using POMQC.Services.DHU;
using POMQC.Services.Inspection;
using POMQC.Utilities;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Checklist;
using POMQC.ViewModels.Dashboard;
using POMQC.ViewModels.Inspection;

namespace POMQC.Controllers.Inspection
{
    public class DHUController : BaseController
    {
        private readonly IChecklistService _checklist = new ChecklistService();
        private readonly IDHUService _dhu = new DHUService();
        private readonly IInspectionService _inspection = new InspectionService();
        private readonly IDashboardService _dashboard = new DashboardService();
        private readonly IDefectService _defect = new DefectService();

        [HttpPost, AuroraAuthorize(GroupId = 8)]
        public ActionResult Upload(DHUEntity data)
        {
            var model = new DHUViewModel();
            model.Item = data;

            var files = new List<string>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var fileNames = Request.Files[i].FileName.Split('.');
                var fileName = fileNames.First() + DateTime.Now.ToString("-yyyyMMddHHmmss.") + fileNames.Last();
                files.Add(fileName.Normalized());
            }
            
            var result = new Result();
            model.Item.DefectImg = string.Join(";", files);
            if (model.Item.DHUId > 0)
            {
                model.Item.UpdatedBy = AuroraSession.UserId;
                model.Item.UpdatedDate = DateTime.Now;

                result = _inspection.UpdateDHU(model);
            }
            else
            {
                model.Item.CreatedBy = AuroraSession.UserId;
                model.Item.CreatedDate = DateTime.Now;

                result = _inspection.InsertDHU(model);
            }

            if (result.Id > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var stream = Request.Files[i].InputStream;
                    SaveImage(stream, files[i]);
                }
            }

            return JsonBase(result);
        }

        public ActionResult Select(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0)
        {
            var data = _dhu.Select(custId, custPO, aiglPO, date, type);
            var result = GetDHUViewModel(data.FirstOrDefault(i => i.Function == (InspectionFunctionViewModel)type && i.Item.CreatedDate == date), (InspectionFunctionViewModel)type, false);

            return JsonBase(result);
        }

        [AuroraAuthorize]
        public ActionResult Export(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0)
        {
            date = date == DateTime.MinValue ? DateTime.Now : date;
            var data = _dhu.Select(custId, custPO, aiglPO, date, type);
            var codes = _defect.DefectCodes();
            var locs = _defect.DefectionLocations();
            var inspection = data.FirstOrDefault(i => i.Function == (InspectionFunctionViewModel)type && i.Item.CreatedDate == date) ?? new DHUViewModel();
            var result = GetDHUViewModel(inspection, (InspectionFunctionViewModel)type, true, codes, locs);

            var excel = RenderRazorViewToString("_DHU", result);

            //Response.Write(excel);

            var fileName = ((InspectionFunctionViewModel)type).ToString().ToUpperInvariant() + "-" +
                custPO + "-" + aiglPO + "-" + (date.HasValue ? date.Value.ToString("MM_dd_yyyy_hhmmssfff") : DateTime.Now.ToString("MM_dd_yyyy_hhmmssfff"));
            return ExportExcel(excel, fileName);
        }        

        public ActionResult View(long custId, string custPO, string aiglPO, string from, string to, string status, int factoryId)
        {
            // Query Inspection
            var model = new DashboardViewModel();
            var inspection = new InspectionViewModel();
            IList<ChecklistViewModel> checklist = new List<ChecklistViewModel>();

            // Set Inspection
            var poss = _dashboard.POs(Convert.ToDateTime(from), Convert.ToDateTime(to), custId, custPO, aiglPO, status);
            model.POs = factoryId == 0 ? poss : poss.Where(p => p.FactoryId == factoryId).ToList();

            var firstPO = model.POs.FirstOrDefault();
            if (firstPO != null)
            {
                custId = custId == 0 ? firstPO.CustId : custId;
                custPO = string.IsNullOrWhiteSpace(custPO) ? firstPO.CustPO : custPO;
                aiglPO = string.IsNullOrWhiteSpace(aiglPO) ? firstPO.AIGLPO : aiglPO;
            }

            inspection = _inspection.Select(custId, custPO, aiglPO);
            checklist = _checklist.Select(custId, custPO, aiglPO, (int)ChecklistType.Fabric);

            model.Fabric = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.Fabric, "FABRIC INSPECTION");
            model.Cutting = inspection.Cutting;
            model.Inline = inspection.Inline;
            model.Endline = inspection.Endline;
            model.Finishing = inspection.Finishing;
            model.Packing = inspection.Packing;
            model.Prefinal = inspection.Prefinal;
            model.Final = inspection.Final; 
            model.Styles = inspection.Styles;

            return JsonBase(model);
        }

        [AuroraAuthorize]
        public ActionResult ExportPdf(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0)
        {
            date = date == DateTime.MinValue ? DateTime.Now : date;
            var data = _dhu.Select(custId, custPO, aiglPO, date, type);
            var codes = _defect.DefectCodes();
            var locs = _defect.DefectionLocations();
            var inspection = data.FirstOrDefault(i => i.Function == (InspectionFunctionViewModel)type && i.Item.CreatedDate == date) ?? new DHUViewModel();
            var result = GetDHUViewModel(inspection, (InspectionFunctionViewModel)type, true, codes, locs);
            var excel = RenderRazorViewToString("_DHU", result); 
            var images = new Dictionary<string, Image>();

            foreach (var item in inspection.Item.Images)
            {
                if (item.hasImage)
                {
                    images.Add(Guid.NewGuid() + "__" + item.name, Image.GetInstance(Server.MapPath(item.url)));
                }                
            }
            
            return ExportPdf(excel,
                ((InspectionFunctionViewModel)type).ToString().ToUpperInvariant() + "-" + 
                custPO + "-" + aiglPO + "-" +
                (date.HasValue ? date.Value.ToString("MM_dd_yyyy_hhmmssfff") : DateTime.Now.ToString("MM_dd_yyyy_hhmmssfff")),
                images);
        }

        [AuroraAuthorize]
        public ActionResult MailTo()
        {
            var query = Request.QueryString;
            var custpo = query["custpo"];
            var aiglpo = query["aiglpo"];
            var date = query["date"].ConvertTo<DateTime>();
            var type = (InspectionFunctionViewModel)query["type"].ConvertTo<int>();

            var model = new MailToViewModel
            {
                From = AuroraSession.Email,
                PathAndQuery = "~/dhu/exportpdf" + Request.Url.Query,
                Attachment = ((InspectionFunctionViewModel)type).ToString().ToUpperInvariant() + "-" +
                    custpo + "-" + aiglpo + "-" +
                    (date == DateTime.MinValue ? DateTime.Now.ToString("MM_dd_yyyy_hhmmssfff") : date.ToString("MM_dd_yyyy_hhmmssfff")) + ".pdf"
            };

            return PartialView(model);
        }

        [HttpPost, AuroraAuthorize]
        public ActionResult ExportPdf(string to, string cc, string bcc, string subject, string body, long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0)
        {
            date = date == DateTime.MinValue ? DateTime.Now : date;
            var data = _dhu.Select(custId, custPO, aiglPO, date, type);
            var codes = _defect.DefectCodes();
            var locs = _defect.DefectionLocations();
            var inspection = data.FirstOrDefault(i => i.Function == (InspectionFunctionViewModel)type && i.Item.CreatedDate == date) ?? new DHUViewModel();
            var result = GetDHUViewModel(inspection, (InspectionFunctionViewModel)type, true, codes, locs);
            var excel = RenderRazorViewToString("_DHU", result);
            var images = new Dictionary<string, Image>();

            foreach (var item in inspection.Item.Images)
            {
                if (item.hasImage)
                {
                    images.Add(Guid.NewGuid() + "__" + item.name, Image.GetInstance(Server.MapPath(item.url)));
                }
            }

            var sent = SendMail(excel, ((InspectionFunctionViewModel)type).ToString().ToUpperInvariant() + "-" +
                custPO + "-" + aiglPO + "-" +
                (date.HasValue ? date.Value.ToString("MM_dd_yyyy_hhmmssfff") : DateTime.Now.ToString("MM_dd_yyyy_hhmmssfff")),
                images,
                ConfigurationManager.AppSettings["Smtp"],
                ConfigurationManager.AppSettings["Port"].ConvertTo<int>(),
                AuroraSession.Email,
                AuroraSession.Epwd,
                to.Split(new string[] { ",", ";", ", ", "; ", " ,", " ;", " , ", " ; " }, StringSplitOptions.RemoveEmptyEntries),
                subject,
                body,
                cc.Split(new string[] { ",", ";", ", ", "; ", " ,", " ;", " , ", " ; " }, StringSplitOptions.RemoveEmptyEntries),
                bcc.Split(new string[] { ",", ";", ", ", "; ", " ,", " ;", " , ", " ; " }, StringSplitOptions.RemoveEmptyEntries));

            return JsonBase(new { isSuccess = string.IsNullOrWhiteSpace(sent), message = sent });
        }

        [AuroraAuthorize(GroupId = 8)]
        public ActionResult Delete(long dhuId, string img)
        {
            try
            {
                System.IO.File.Delete(Server.MapPath("~/Images/Uploads/" + img));
            }
            catch
            {
            }

            _inspection.DeleteDHU(dhuId, img);
            return Json(new { IsSuccess = true });
        }
    }
}