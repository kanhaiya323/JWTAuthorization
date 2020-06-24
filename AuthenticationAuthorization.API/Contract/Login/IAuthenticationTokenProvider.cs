using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.API.Contract.User
{
    public interface IAuthenticationTokenProvider
    {
        void Create(AuthenticationTokenCreateContext context);
        Task CreateAsync(AuthenticationTokenCreateContext context);
        void Receive(AuthenticationTokenReceiveContext context);
        Task ReceiveAsync(AuthenticationTokenReceiveContext context);
    }
}
