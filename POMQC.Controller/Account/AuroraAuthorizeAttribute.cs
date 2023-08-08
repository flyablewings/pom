using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using POMQC.Entities.Base;
using POMQC.ViewModels.Filter;

namespace POMQC.Controllers.Account
{
    public class AuroraAuthorizeAttribute : AuthorizeAttribute
    {
        public FunctionViewModel Function { get; set; }

        public int GroupId { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var currentSession = new CurrentSession();
            if (currentSession.UserId == 0 || !filterContext.HttpContext.Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
                return;
            }

            if (GroupId != 0 && 
                Function != FunctionViewModel.Sample)
            {
                if (currentSession.GroupId != GroupId && currentSession.GroupId != 1 && currentSession.GroupId != 8)
                {
                    filterContext.Result = new JsonNetResult
                    {
                        ContentEncoding = Encoding.UTF8,
                        ContentType = "application/json",
                        Data = new Result { Id = 0, Message = "Access denied. Only Accounting or QA/QC Group (Group_SID=1/8) can perform this action" },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue,
                        RecursionLimit = int.MaxValue
                    };
                }
            }
        }
    }
}