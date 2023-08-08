using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using Newtonsoft.Json;
using POMQC.Controllers.Account;
using POMQC.Entities.Base;
using POMQC.Entities.Final;
using POMQC.Services.Defect;
using POMQC.Services.Final;
using POMQC.Services.Inspection;
using POMQC.Utilities;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Dashboard;
using POMQC.ViewModels.Inspection;

namespace POMQC.Controllers.Final
{
    public class FinalController : BaseController
    {
        private readonly IInspectionService _service = new InspectionService();
        private readonly IFinalService _final = new FinalService();
        private readonly IDefectService _defect = new DefectService();

        [HttpPost, AuroraAuthorize(GroupId = 8)]
        public ActionResult Upload(FinalEntity data)
        {
            var model = new FinalViewModel();
            model.Item = data;
            model.POs = JsonConvert.DeserializeObject<IList<FinalCustPOEntity>>(data.EntitiesString);

            var files = new List<string>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var fileNames = Request.Files[i].FileName.Split('.');
                var fileName = fileNames.First() + DateTime.Now.ToString("-yyyyMMddHHmmss.") + fileNames.Last();
                files.Add(fileName.Normalized());
            }

            var result = new Result();
            model.Item.MeasurementPackingAudit = string.Join(";", files);
            if (model.Item.FinalId > 0)
            {
                model.Item.UpdatedBy = AuroraSession.UserId;
                model.Item.UpdatedDate = DateTime.Now;

                result = _service.UpdateFinal(model);
            }
            else
            {
                model.Item.CreatedBy = AuroraSession.UserId;
                model.Item.CreatedDate = DateTime.Now;

                result = _service.InsertFinal(model);
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
            var data = _final.Select(custId, custPO, aiglPO, date, type);
            var final = data.FirstOrDefault(i => i.Function == (InspectionFunctionViewModel)type && i.Item.CreatedDate == date);
            var result = GetFinalViewModel(final, (InspectionFunctionViewModel)type, false, final.POs);

            return JsonBase(result);
        }

        [AuroraAuthorize]
        public ActionResult Export(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0)
        {
            date = date == DateTime.MinValue ? DateTime.Now : date;
            var data = _final.Select(custId, custPO, aiglPO, date, type);
            var codes = _defect.DefectCodes();
            var locs = _defect.DefectionLocations();
            var inspection = data.FirstOrDefault(i => i.Function == (InspectionFunctionViewModel)type && i.Item.CreatedDate == date) ?? new FinalViewModel();
            var result = GetFinalViewModel(inspection, (InspectionFunctionViewModel)type, true, inspection.POs, codes, locs);

            var excel = RenderRazorViewToString("_Final", result);
            //Response.Write(excel);

            var fileName = ((InspectionFunctionViewModel)type).ToString().ToUpperInvariant() + "-" +
                custPO + "-" + aiglPO + "-" + (date.HasValue ? date.Value.ToString("MM_dd_yyyy_hhmmssfff") : DateTime.Now.ToString("MM_dd_yyyy_hhmmssfff"));
            return ExportExcel(excel, fileName);
        }

        [AuroraAuthorize]
        public ActionResult ExportPdf(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0)
        {
            date = date == DateTime.MinValue ? DateTime.Now : date;
            var data = _final.Select(custId, custPO, aiglPO, date, type);
            var codes = _defect.DefectCodes();
            var locs = _defect.DefectionLocations();
            var inspection = data.FirstOrDefault(i => i.Function == (InspectionFunctionViewModel)type && i.Item.CreatedDate == date) ?? new FinalViewModel();
            var result = GetFinalViewModel(inspection, (InspectionFunctionViewModel)type, true, inspection.POs, codes, locs);

            var excel = RenderRazorViewToString("_Final", result);
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
                PathAndQuery = "~/final/exportpdf" + Request.Url.Query,
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
            var data = _final.Select(custId, custPO, aiglPO, date, type);
            var codes = _defect.DefectCodes();
            var locs = _defect.DefectionLocations();
            var inspection = data.FirstOrDefault(i => i.Function == (InspectionFunctionViewModel)type && i.Item.CreatedDate == date) ?? new FinalViewModel();
            var result = GetFinalViewModel(inspection, (InspectionFunctionViewModel)type, true, inspection.POs, codes, locs);

            var excel = RenderRazorViewToString("_Final", result);
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
        public ActionResult Delete(long finalId, string img)
        {
            try
            {
                System.IO.File.Delete(Server.MapPath("~/Images/Uploads/" + img));
            }
            catch
            {
            }

            _service.DeleteFinal(finalId, img);
            return Json(new { IsSuccess = true });
        }
    }
}