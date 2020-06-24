using AuthenticationAuthorization.Contract.User;
using AuthenticationAuthorization.Core.Users;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace AuthenticationAuthorization.API.AuthenticationProvider
{
    public class AuthProvider: OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string symmetricKeyAsBase64 = string.Empty;
            if(!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if(context == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                return Task.FromResult<object>(null);
            }
            context.OwinContext.Set<string>("as:clientAllowedOrigin", ConfigurationManager.AppSettings["AllowedCORS"]);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", ConfigurationManager.AppSettings["RefreshTokenLifeTime"]);
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            
            var user = GetUserDbService().FindUser(context.UserName, context.Password);
            if(user== null)
            {
                context.SetError("invalid_grant", "This username or password is incorrect");
                return Task.FromResult<object>(null);
            }
            //else
            //{
            //    if(user.IsActive == false)
            //    {
            //        context.SetError("invalid_grant", "User is not active");
            //        return Task.FromResult<object>(null);
            //    }
            //    if (!(DateTime.Now > user.EffectiveDt && DateTime.Now < user.ExpiryDt))
            //    {
            //        context.SetError("invalid_grant", "This user account has expired.");
            //        return Task.FromResult<object>(null);
            //    }      
            //}

            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            var props = new AuthenticationProperties
           (
               new Dictionary<string, string>
               {
                    { "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId },
                    { "userName", (context.UserName == null) ? string.Empty : context.UserName }
               }
           );

            props.IssuedUtc = DateTime.UtcNow;
            props.ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiryInMinutes"]));

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        } 


        /// <summary>
        /// Return interface [Login] refernce to make all business logic
        /// </summary>
        /// <returns></returns>
        private ILoginService GetUserDbService()
        {
            #region Comment : Here UserDbService interface refernce to do/make process all business logic
            ILoginService service = new LoginService();
            #endregion
            return service;
        }
    }
}