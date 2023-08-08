using System.Web.Mvc;
using System.Web.Security;
using POMQC.Services.Account;
using POMQC.ViewModels.Account;

namespace POMQC.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly ILoginService _login = new LoginService();

        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost, ActionName("Login")]
        public ActionResult LoginPost(LoginViewModel model)
        {
            var loginResult = _login.Login(model.Login.Username ?? string.Empty, model.Login.Password ?? string.Empty);
            if (string.IsNullOrEmpty(loginResult.ErrorMessage))
            {
                var currentSession = new CurrentSession();
                currentSession.UserId = loginResult.Login.UserId;
                currentSession.Username = loginResult.Login.Username;
                currentSession.GroupId = loginResult.Login.GroupId;
                currentSession.Email = loginResult.Login.Email;
                currentSession.Epwd = loginResult.Login.Epwd;

                FormsAuthentication.SetAuthCookie(model.Login.Username, false);
                return Redirect(Request["ReturnUrl"] ?? "~/");
            }

            return View(loginResult);
        }

        [HttpGet, AuroraAuthorize]
        public ActionResult ConfigEmail()
        {
            var auroraSession = CurrentSession.Instance;
            return PartialView(new EmailViewModel { Email = auroraSession.Email, Password = auroraSession.Epwd });
        }

        [HttpPost, AuroraAuthorize]
        public ActionResult ConfigEmailPost(EmailViewModel model)
        {
            var auroraSession = CurrentSession.Instance;
            var result = _login.UpdateEmail(auroraSession.UserId, model.Email, model.Password);

            if (result.IsSuccess)
            {
                auroraSession.Email = model.Email;
                auroraSession.Epwd = model.Password;
            }

            return Json(result);
        }

        public ActionResult Checkin()
        {
            var isAuthenticated = CurrentSession.Instance.UserId != 0 && Request.IsAuthenticated;
            return Json(isAuthenticated);
        }
    }
}