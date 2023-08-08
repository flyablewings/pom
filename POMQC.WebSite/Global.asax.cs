using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using POMQC.Utilities;

namespace POMQC.WebSite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Fos.Data.Configuration.DbSettingProviderManager.Current.Start();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var mail = "Message: {0} <br />" +
                "Url: {1} <br />" +
                "Ref: {2} <br />" +
                "Form: {3} <br />" +
                "Stack trace: {4} <br />" +
                "Target site: {5} <br />" +
                "Inner exception: {6} <br />" +
                "IP: {7}";
            var ex = Server.GetLastError();
            Task.Factory.StartNew(() => Utils.SendMail(
                Utils.MailServer,
                Utils.MailPort,
                Utils.MailAdmin,
                Utils.MailPwd,
                new string[] { Utils.MailError },
                "POMQC error! " + ex.Message,
                string.Format(mail,
                    ex.Message,
                    Request.Url.PathAndQuery,
                    Request.UrlReferrer,
                    Request.Form,
                    ex.StackTrace,
                    ex.TargetSite,
                    ex.InnerException,
                    Request.UserHostAddress)));
        }
    }
}