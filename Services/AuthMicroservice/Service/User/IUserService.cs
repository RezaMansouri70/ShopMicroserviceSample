using ApplicationServices.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainClass.User;

namespace ApplicationServices.Services.UserService
{
    public interface IUserService
    {
        bool CanLogin(LoginUserDto loginModel);
        User Register(AddUserDto addUserModel);
        User GetUserByMobile(string mobile);
        Dictionary<string, object> GetUserClaim(string mobile);

    }
}
