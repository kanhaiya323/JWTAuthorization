using AuthenticationAuthorization.API.Models;
using AuthenticationAuthorization.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace AuthenticationAuthorization.Contract.User
{
    public interface ILoginService
    {
        UserDomain FindUser(string userName, string password);
        Task<bool> AddRefreshToken(RefreshToken refreshToken);
        Task<RefreshToken> FindRefreshToken(string hashedTokenId);
        Task<bool> RemoveRefreshToken(string hashedTokenId);
    }
}