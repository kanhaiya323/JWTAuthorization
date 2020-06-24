using AuthenticationAuthorization.API.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationAuthorization.Domain;

namespace AuthenticationAuthorization.API.Contract.User
{
    public interface IUserService
    {
        ApiResponse RegistreUser(UserDomain user);
        int CheckEmail(string inputEmail);
    }
}
