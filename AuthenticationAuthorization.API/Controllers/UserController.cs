using AuthenticationAuthorization.API.Contract.User;
using AuthenticationAuthorization.API.Core.User;
using AuthenticationAuthorization.API.Helper;
using AuthenticationAuthorization.API.Response;
using AuthenticationAuthorization.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AuthenticationAuthorization.API.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class UserController : ApiController
    {
        [AllowAnonymous]
        [Route("Register")]
        public ApiResponse Post(UserDomain user)
        {
            user.Password = HashGenerator.GetHash(user.Password);
            return GetUserService().RegistreUser(user);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("checkEmail")]
        public int EmailCheck(string inputEmail)
        {
            return GetUserService().CheckEmail(inputEmail);
        }

        // Db Service 
        public IUserService GetUserService()
        {
            IUserService userService = new UserService();
            return userService;
        }
    }
}
