using System.Web.Mvc;

namespace POMQC.Controllers
{
    public class SharedController : Controller
    {
        public ActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}