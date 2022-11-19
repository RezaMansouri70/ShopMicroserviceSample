using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Microservices.Web.Frontend.Controllers
{

    public class AuthenticationController : Controller
    {

        private IConfiguration _configuration;

        public AuthenticationController(IConfiguration Configuration)
        {
            _configuration = Configuration;
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("Auth");
            return RedirectToAction("Index", "Home", "");

        }

        public IActionResult Login()
        {
            return View(new LoginDTO());
        }

        [HttpPost]
        public IActionResult Login(LoginDTO model)
        {
            var restClient = new RestClient();
            var request = new RestRequest(_configuration["MicroservicAddress:ApiGatewayForWeb:Uri"] + "Auth/Login", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            string serializeModel = JsonSerializer.Serialize(model);
            request.AddParameter("application/json", serializeModel, ParameterType.RequestBody);
            var response = restClient.Execute(request);
            if (string.IsNullOrEmpty(response.Content)) 
            {

                ViewBag.Message = " Login Faild ";
                return View(model);
            }

            var token = JsonSerializer.Deserialize<AuthResult>(response.Content);



            if (token.status)
            {
                var cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTime.Now.AddDays(1);
                cookieOptions.Path = "/";
                Response.Cookies.Append("Auth", token.token, cookieOptions);

                return RedirectToAction("Index", "Home", "");

            }
            else
            {
                ViewBag.Message = " Login Faild ";
                return View(model);
            }

        }

        public class LoginDTO
        {
            public string Mobile { get; set; }
            public string Password { get; set; }

        }
        public class AuthResult
        {
            public string token { get; set; }
            public bool status { get; set; }

        }


    }


}
