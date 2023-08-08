using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using POMQC.Controllers.Account;
using POMQC.Entities.Base;
using POMQC.Services.Checklist;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Checklist;
using POMQC.ViewModels.Filter;
using POMQC.Utilities;

namespace POMQC.Controllers.Checklist
{
    public class ChecklistController : BaseController
    {
        private readonly IChecklistService _service = new ChecklistService();

        [HttpPost, AuroraAuthorize(GroupId = 8)]
        public ActionResult Upload(ChecklistViewModel data)
        {
            var files = new List<string>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var fileNames = Request.Files[i].FileName.Split('.');
                var fileName = fileNames.First() + DateTime.Now.ToString("-yyyyMMddHHmmss.") + fileNames.Last();
                files.Add(fileName.Normalized());
            }

            data.Doc = string.Join(";", files);
            var result = new Result();
            if (data.IsUpdate)
            {
                data.UpdatedBy = AuroraSession.UserId;
                data.UpdatedDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));

                result = _service.Update(data);
            }
            else
            {
                data.CreatedBy = AuroraSession.UserId;
                data.CreatedDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));

                result = _service.Insert(data);
            }

            if (result.IsSuccess)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (Utilities.Utils.ImageFileTypes.Split(',').Any(img => files[i].EndsWith(img, StringComparison.OrdinalIgnoreCase)))
                    {
                        var stream = Request.Files[i].InputStream;
                        SaveImage(stream, files[i]);
                    }
                    else
                    {
                        Request.Files[i].SaveAs(Server.MapPath("~/Images/Uploads/" + files[i]));
                    }                    
                }
            }

            return JsonBase(result);
        }

        [HttpPost, AuroraAuthorize(GroupId = 8, Function = FunctionViewModel.Sample)]
        public ActionResult Sample(ChecklistViewModel data)
        {
            var files = new List<string>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var fileName = Request.Files[i].FileName.Split('.').First() + DateTime.Now.ToString("-yyyyMMddHHmmss.") + Request.Files[i].FileName.Split('.').Last();
                files.Add(fileName.Normalized());
            }

            data.Doc = string.Join(";", files);
            var result = new Result();
            if (data.IsUpdate)
            {
                data.UpdatedBy = AuroraSession.UserId;
                data.UpdatedDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));

                result = _service.Update(data);
            }
            else
            {
                data.CreatedBy = AuroraSession.UserId;
                data.CreatedDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));

                result = _service.Insert(data);
            }

            if (result.IsSuccess)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (Utilities.Utils.ImageFileTypes.Split(',').Any(img => files[i].EndsWith(img, StringComparison.OrdinalIgnoreCase)))
                    {
                        var stream = Request.Files[i].InputStream;
                        SaveImage(stream, files[i]);
                    }
                    else
                    {
                        Request.Files[i].SaveAs(Server.MapPath("~/Images/Uploads/" + files[i]));
                    } 
                }
            }

            return JsonBase(result);
        }

        public ActionResult View(long custId = 0, string custPO = "", string aiglPO = "")
        {
            var result = _service.Select(custId, custPO, aiglPO);
            return JsonBase(result);
        }

        public ActionResult ViewDetail(long custId, string custPO, string aiglPO)
        {
            var result = _service.Select(custId, custPO, aiglPO);
            return JsonBase(result);
        }

        public FileStreamResult ViewFile(string file)
        {
            var fileStream = new FileStream(Server.MapPath("~/Images/Uploads/" + file),
                FileMode.Open,
                FileAccess.Read
            );

            var fsResult = new FileStreamResult(fileStream, FileMapping.GetMimeType(file.Split('.').Last()));
            fsResult.FileDownloadName = file;
            
            return fsResult;
        }

        [AuroraAuthorize(GroupId = 8)]
        public ActionResult Delete(long custId, string custPO, string aiglPO, string doc, int type)
        {
            try
            {
                System.IO.File.Delete(Server.MapPath("~/Images/Uploads/" + doc));
            }
            catch
            {
            }

            _service.Delete(custId, custPO, aiglPO, doc, type);
            return Json(new { IsSuccess = true });
        }
    }
}