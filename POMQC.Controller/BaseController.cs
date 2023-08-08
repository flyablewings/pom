using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.SessionState;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using POMQC.Controllers.Account;
using POMQC.Entities.Defect;
using POMQC.Entities.Final;
using POMQC.Services.Final;
using POMQC.Utilities;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Checklist;
using POMQC.ViewModels.Inspection;

namespace POMQC.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BaseController : Controller
    {
        private IFinalService _final = new FinalService();
        protected CurrentSession AuroraSession { get { return new CurrentSession(); } }

        protected JsonResult JsonBase(object data)
        {
            return new JsonNetResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue,
                RecursionLimit = int.MaxValue 
            };
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding)
        {
            return new JsonNetResult
            {
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                Data = data,
                MaxJsonLength = int.MaxValue,
                RecursionLimit = int.MaxValue 
            };
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult 
            { 
                ContentEncoding = contentEncoding, 
                ContentType = contentType, 
                Data = data, 
                JsonRequestBehavior = JsonRequestBehavior.AllowGet, 
                MaxJsonLength = int.MaxValue, 
                RecursionLimit = int.MaxValue 
            };
        }

        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected ChecklistViewModel GetChecklistViewModel(IList<ChecklistViewModel> items, ChecklistFunctionViewModel function, string title, bool isActiveTab = false)
        {
            var result = items.FirstOrDefault(i => i.Function == function);
            result = result ?? new ChecklistViewModel();
            result.Function = function;
            result.Title = title;
            result.IsActiveTab = isActiveTab;

            return result;
        }

        protected DHUViewModel GetDHUViewModel(DHUViewModel model, InspectionFunctionViewModel function, bool isReadOnly = false, IList<DefectCode> codes = null, IList<DefectLocation> locs = null)
        {
            var result = model ?? new DHUViewModel();
            result.Function = function;
            result.IsReadOnly = isReadOnly;
            result.DefectCodes = codes;
            result.DefectLocations = locs;

            return result;
        }

        protected FinalViewModel GetFinalViewModel(FinalViewModel model, InspectionFunctionViewModel function, bool isReadOnly = false, IList<FinalCustPOEntity> pos = null, IList<DefectCode> codes = null, IList<DefectLocation> locs = null)
        {            
            var result = model ?? new FinalViewModel();
            var styles = _final.Select(result.Item.AIGLPO ?? string.Empty, result.Item.FinalId);
            result.Function = function;
            result.IsReadOnly = isReadOnly;
            result.POs = styles;
            result.DefectCodes = codes;
            result.DefectLocations = locs;

            return result;
        }

        protected string GenerateImage(string image)
        {
            var bmp = new Bitmap(Server.MapPath("~/Images/Uploads/" + image));
            var g = Graphics.FromImage(bmp);

            g.DrawImage(System.Drawing.Image.FromFile(Server.MapPath("~/Images/Uploads/" + image)), 0, 0);

            var stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Jpeg);

            var bytes = stream.ToArray();
            var img = Convert.ToBase64String(bytes);

            return "data:image/jpeg;base64," + img;
        }

        protected ActionResult ExportExcel(string excel, string fileName)
        {
            fileName = fileName.Normalized();
            var fileStream = new FileStream(Server.MapPath("~/Images/Downloads/" + fileName + ".xls"), FileMode.OpenOrCreate);
            var writer = new StreamWriter(fileStream);
            writer.Write(excel);
            writer.Close();
            fileStream.Close();

            return Redirect("~/Images/Downloads/" + fileName + ".xls");
        }
        
        protected ActionResult ExportPdf(string htmlTable, string fileName, IDictionary<string, iTextSharp.text.Image> images)
        {
            try
            {
                fileName = fileName.Normalized();
                FontFactory.Register(Server.MapPath("~/fonts/vuArial.ttf"));
                StyleSheet style = new StyleSheet();
                style.LoadTagStyle("td", "face", "VU Arial");
                style.LoadTagStyle("td", "encoding", BaseFont.IDENTITY_H);

                var fileStream = new FileStream(Server.MapPath("~/Images/Downloads/" + fileName + ".pdf"), FileMode.OpenOrCreate);
                
                //Set page size as A4
                Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);

                //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);                
                PdfWriter.GetInstance(pdfDoc, fileStream);

                //Open PDF Document to write data
                pdfDoc.Open();

                //Read string contents using stream reader and convert html to parsed conent
                var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(htmlTable), style);

                var logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/logo.png"));
                pdfDoc.Add(logo);
                
                //Get each array values from parsed elements and add to the PDF document
                foreach (var htmlElement in parsedHtmlElements)
                {
                    pdfDoc.Add(htmlElement);
                }

                if (images.Count > 0)
                {
                    pdfDoc.Add(new Paragraph("DEFECT IMAGES"));
                }

                var width = ConfigurationManager.AppSettings["PdfImageWidth"].ConvertTo<float>();
                var height = ConfigurationManager.AppSettings["PdfImageHeight"].ConvertTo<float>();

                foreach (KeyValuePair<string, iTextSharp.text.Image> image in images)
                {
                    // Add image
                    var img = image.Value;
                    img.ScaleAbsolute(width, height);
                    pdfDoc.Add(img);
                }

                //Close your PDF
                pdfDoc.Close();

                return Redirect("~/Images/Downloads/" + fileName + ".pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected string SendMail(string htmlTable, string fileName, IDictionary<string, iTextSharp.text.Image> images, string smtp, int port, string from, string pwd, string[] to, string subject, string content, string[] cc = null, string[] bcc = null)
        {
            try
            {
                fileName = fileName.Normalized();
                FontFactory.Register(Server.MapPath("~/fonts/vuArial.ttf"));
                StyleSheet style = new StyleSheet();
                style.LoadTagStyle("td", "face", "VU Arial");
                style.LoadTagStyle("td", "encoding", BaseFont.IDENTITY_H);

                //Set page size as A4
                Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfWriter.GetInstance(pdfDoc, memoryStream);

                    //Open PDF Document to write data
                    pdfDoc.Open();

                    //Assign Html content in a string to write in PDF
                    string contents = htmlTable;

                    //Read string contents using stream reader and convert html to parsed conent
                    var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(contents), style);

                    //Get each array values from parsed elements and add to the PDF document
                    foreach (var htmlElement in parsedHtmlElements)
                        pdfDoc.Add(htmlElement);

                    if (images.Count > 0)
                    {
                        pdfDoc.Add(new Paragraph("DEFECT IMAGES"));
                    }

                    var width = ConfigurationManager.AppSettings["PdfImageWidth"].ConvertTo<float>();
                    var height = ConfigurationManager.AppSettings["PdfImageHeight"].ConvertTo<float>();

                    foreach (KeyValuePair<string, iTextSharp.text.Image> image in images)
                    {
                        // Add image
                        var img = image.Value;
                        img.ScaleAbsolute(width, height);
                        pdfDoc.Add(img);
                    }

                    //Close your PDF
                    pdfDoc.Close();
                    byte[] bytes = memoryStream.ToArray();
                    memoryStream.Close();

                    var attachments = new List<Attachment>();
                    attachments.Add(new Attachment(new MemoryStream(bytes), fileName + ".pdf", "application/pdf"));
                    Utilities.Utils.SendMail(smtp, port, from, pwd, to, subject, content, cc, bcc, attachments, true);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        protected void SaveImage(Stream stream, string fileName)
        {
            using (var img = System.Drawing.Image.FromStream(stream))
            {
                var ratio = ConfigurationManager.AppSettings["ResizeImage"].ConvertTo<double>();
                var thumbImg = new Bitmap((int)(img.Width * ratio), (int)(img.Height * ratio));
                var thumbGraph = Graphics.FromImage(thumbImg);

                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                var imgRectangle = new System.Drawing.Rectangle(0, 0, (int)(img.Width * ratio), (int)(img.Height * ratio));
                thumbGraph.DrawImage(img, imgRectangle);
                thumbImg.Save(Server.MapPath("~/Images/Uploads/" + fileName));
                thumbImg.Dispose();
                thumbGraph.Dispose();
            }
        }
    }
}