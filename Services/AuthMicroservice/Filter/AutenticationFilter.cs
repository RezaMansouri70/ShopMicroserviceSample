using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SharedService.Service;

namespace AuthMicroservice.Filter
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
            base.OnActionExecuting(context);
        }
    }

}
