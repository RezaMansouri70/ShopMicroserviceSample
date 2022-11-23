using Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SharedService.Service
{

    public static class TokenManagerService
    {



        public static string CreateToken(Dictionary<string, object> keyValues)
        {

            DateTime issuedAt = DateTime.UtcNow;
            DateTime expires = DateTime.UtcNow.AddYears(1);
            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();// new[]
            foreach (var key in keyValues)
            {
                claimsIdentity.AddClaim(new Claim(key.Key, key.Value.ToString()));
            }
            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var token = tokenHandler.CreateJwtSecurityToken(
                        issuer: "http://localhost:7147",
                        audience: "http://localhost:7147",
                        subject: claimsIdentity,
                        notBefore: issuedAt,
                        expires: expires,
                        signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);
            tokenString = EncryptionHelper.Encrypt(tokenString);

            return tokenString;

        }



        public static UserLoginInfo GetUserInfo(HttpContextAccessor _httpContextAccessor)
        {

            var response = new UserLoginInfo();
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["token"].ToString();
                if (token == "")
                    return response;

                string decryptedtoken = EncryptionHelper.Decrypt(token);
                token = decryptedtoken;
                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                securityToken = handler.ReadToken(token);
                var tokenDecodeJwt = handler.ReadJwtToken(token);
                response.Token = token;
                response.UserID = Guid.Parse(tokenDecodeJwt.Claims.FirstOrDefault(a => a.Type == "UserID").Value);
                response.UserName = tokenDecodeJwt.Claims.FirstOrDefault(a => a.Type == "UserName").Value;
                response.Roles = tokenDecodeJwt.Claims.FirstOrDefault(a => a.Type == "Roles").Value;
                response.LoginDate = DateTime.Parse(tokenDecodeJwt.Claims.FirstOrDefault(a => a.Type == "LoginDate").Value);
                response.ExpireDate = DateTime.Parse(tokenDecodeJwt.Claims.FirstOrDefault(a => a.Type == "ExpireDate").Value);

            }
            catch (Exception)
            {

            }

            return response;
        }

        public static UserLoginInfo GetUserInfo(string token)
        {

            var response = new UserLoginInfo();
            try
            {
                if (token == "")
                    return response;

                string decryptedtoken = EncryptionHelper.Decrypt(token);
                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                securityToken = handler.ReadToken(decryptedtoken);
                var tokenDecodeJwt = handler.ReadJwtToken(decryptedtoken);
                response.Token = token;
                response.UserID = Guid.Parse(tokenDecodeJwt.Claims.FirstOrDefault(a => a.Type == "UserID").Value);
                response.UserName = tokenDecodeJwt.Claims.FirstOrDefault(a => a.Type == "UserName").Value;
                response.Roles = tokenDecodeJwt.Claims.FirstOrDefault(a => a.Type == "Roles").Value;
                response.LoginDate = DateTime.Parse(tokenDecodeJwt.Claims.FirstOrDefault(a => a.Type == "LoginDate").Value);
                response.ExpireDate = DateTime.Parse(tokenDecodeJwt.Claims.FirstOrDefault(a => a.Type == "ExpireDate").Value);

            }
            catch (Exception)
            {

            }

            return response;
        }


        public static UserLoginInfo GetCurrentUser(HttpRequest request)
        {
            var Authcookie = request.Cookies["Auth"];
            return TokenManagerService.GetUserInfo(Authcookie);
        }



    }
    public class UserLoginInfo
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Roles { get; set; }
        public string Token { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime ExpireDate { get; set; }


    }

}
