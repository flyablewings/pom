using System.Web;
using POMQC.Utilities;

namespace POMQC.Controllers.Account
{
    public class CurrentSession
    {
        public int UserId
        {
            get { return HttpContext.Current.Session["UserId"].ConvertTo<int>(); }
            set { HttpContext.Current.Session["UserId"] = value; }
        }

        public int GroupId
        {
            get { return HttpContext.Current.Session["GroupId"].ConvertTo<int>(); }
            set { HttpContext.Current.Session["GroupId"] = value; }
        }

        public string Username
        {
            get { return HttpContext.Current.Session["Username"].ConvertTo<string>(); }
            set { HttpContext.Current.Session["Username"] = value; }
        }

        public static CurrentSession Instance { get { return new CurrentSession(); } }

        public string Email
        {
            get { return HttpContext.Current.Session["Email"].ConvertTo<string>(); }
            set { HttpContext.Current.Session["Email"] = value; }
        }

        public string Epwd
        {
            get { return HttpContext.Current.Session["Epwd"].ConvertTo<string>(); }
            set { HttpContext.Current.Session["Epwd"] = value; }
        }
    }
}