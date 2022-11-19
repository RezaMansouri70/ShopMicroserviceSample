using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Newtonsoft.Json.Linq;
using SharedService.Service;

namespace AuthMicroservice.Filter
{
    public class AutenticationFilter : ActionFilterAttribute
    {

        public string Roles { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var Authcookie = context.HttpContext.Request.Cookies["Auth"];
            if (Authcookie == null)
            {
                context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(new { controller = "Authentication", action = "Login" })
                    );
            }
            else
            {
                var userInfo = TokenManagerService.GetUserInfo(Authcookie);
                if (string.IsNullOrEmpty(userInfo.UserName))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult("Pelase Send Valid Token In Request Header :| "); ;
                }
            }
            base.OnActionExecuting(context);
        }



    }

}
