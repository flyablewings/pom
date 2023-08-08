using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace POMQC.Controllers
{
    public class JsonNetResult : System.Web.Mvc.JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = ContentType ?? "application/json";

            response.ContentEncoding = ContentEncoding ?? Encoding.UTF8;

            var serializedObject = JsonConvert.SerializeObject(Data);
            response.Write(serializedObject);
        }
    }
}