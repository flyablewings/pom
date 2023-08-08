using System;
using System.Linq;
using System.Web.Mvc;
using POMQC.Controllers.Account;
using POMQC.Entities.Defect;
using POMQC.Services.Defect;

namespace POMQC.Controllers.Defect
{
    public class DefectController : BaseController
    {
        private readonly IDefectService _defect = new DefectService();

        [AuroraAuthorize]
        public ActionResult Category(int id = 0)
        {
            var model = new DefectCategory();
            if (id != 0)
            {
                model = _defect.DefectCategories().FirstOrDefault(i => i.CatId == id);
            }
            
            return View(model);
        }

        [AuroraAuthorize]
        public ActionResult Code(int id = 0)
        {
            var defectCategories = _defect.DefectCategories();
            var model = new DefectCode();
            if (id != 0)
            {
                model = _defect.DefectCodes().FirstOrDefault(i => i.DefId == id);
            }

            model.Categories = defectCategories;

            return View(model);
        }

        [AuroraAuthorize]
        public ActionResult Location(int id = 0)
        {
            var model = new DefectLocation();
            if (id != 0)
            {
                model = _defect.DefectionLocations().FirstOrDefault(i => i.LocId == id);
            }

            return View(model);
        }

        [HttpPost, AuroraAuthorize(GroupId = 8)]
        public ActionResult AddCategory(DefectCategory data)
        {
            data.CreatedBy = AuroraSession.UserId;
            data.CreatedDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            var result = _defect.InsertCategory(data);
            return JsonBase(result);
        }

        [HttpPost, AuroraAuthorize(GroupId = 8)]
        public ActionResult EditCategory(DefectCategory data)
        {
            data.UpdatedBy = AuroraSession.UserId;
            data.UpdatedDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            var result = _defect.UpdateCategory(data);
            return JsonBase(result);
        }
        
        [HttpPost, AuroraAuthorize(GroupId = 8)]
        public ActionResult AddCode(DefectCode data)
        {
            data.CreatedBy = AuroraSession.UserId;
            data.CreatedDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            var result = _defect.InsertCode(data);
            return JsonBase(result);
        }

        [HttpPost, AuroraAuthorize(GroupId = 8)]
        public ActionResult EditCode(DefectCode data)
        {
            data.UpdatedBy = AuroraSession.UserId;
            data.UpdatedDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            var result = _defect.UpdateCode(data);
            return JsonBase(result);
        }

        [HttpPost, AuroraAuthorize(GroupId = 8)]
        public ActionResult AddLocation(DefectLocation data)
        {
            data.CreatedBy = AuroraSession.UserId;
            data.CreatedDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            var result = _defect.InsertLocation(data);
            return JsonBase(result);
        }

        [HttpPost, AuroraAuthorize(GroupId = 8)]
        public ActionResult EditLocation(DefectLocation data)
        {
            data.UpdatedBy = AuroraSession.UserId;
            data.UpdatedDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            var result = _defect.UpdateLocation(data);
            return JsonBase(result);
        }
    }
}