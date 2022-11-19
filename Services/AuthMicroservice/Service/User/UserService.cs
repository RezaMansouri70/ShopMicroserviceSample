using ApplicationServices.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DomainClass.User;
using DataLayer.SqlServer.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ApplicationServices.Services.UserService
{

    public class UserService : IUserService
    {
        private readonly ApplicationContext context;

        public UserService(ApplicationContext context)
        {
            this.context = context;
        }

        public bool CanLogin(LoginUserDto loginModel)
        {
            var user = context.Users.FirstOrDefault(x => x.Mobile == loginModel.Mobile);
            if (user != null)
            {
                return Security.SecurePasswordHasher.Verify(loginModel.Password, user.Password);
            }
            return false;
        }

        public Dictionary<string, object> GetUserClaim(string mobile)
        {

            var user = context.Users.FirstOrDefault(x => x.Mobile == mobile);
            var userInfo = new Dictionary<string, object>();
            userInfo.Add("UserID", user.UserID);
            userInfo.Add("UserName", user.Name);
            userInfo.Add("Roles", user.Roles);
            userInfo.Add("LoginDate", DateTime.Now);
            userInfo.Add("ExpireDate", DateTime.Now.AddHours(24));
            return userInfo;
        }

        public User GetUserByMobile(string mobile)
        {
            return context.Users.FirstOrDefault(x => x.Mobile == mobile);

        }

        public User Register(AddUserDto addUserModel)
        {

            var hashpassword = Security.SecurePasswordHasher.Hash(addUserModel.Password);

            var user = new User()
            {
                UserID = Guid.NewGuid(),
                Mobile = addUserModel.Mobile,
                Name = addUserModel.Name,
                Password = hashpassword,
                Roles = "User",
                CreatedDate = DateTime.Now,
                IsActive = true

            };
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }
    }
}
