using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SharedService.Service;

namespace Filter
{
    public class AutenticationFilter : ActionFilterAttribute
    {

        public string Roles { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            HttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

            var userInfo = TokenManagerService.GetUserInfo(_httpContextAccessor);
            if (string.IsNullOrEmpty(userInfo.UserName))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult("Pelase Send Valid Token In Request Header :| "); ;
            }
            if (!string.IsNullOrEmpty(Roles) && !string.IsNullOrEmpty(userInfo.UserName))
            {
                var AllUserrole = userInfo.Roles?.Split(",");
                if ( !AllUserrole.Contains(Roles))
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult("This User Must have Role " + Roles + " :| "); ;
                }
            }
            base.OnActionExecuting(context);
        }
    }

}
