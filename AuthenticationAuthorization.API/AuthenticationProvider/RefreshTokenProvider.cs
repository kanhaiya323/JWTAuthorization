using AuthenticationAuthorization.API.Helper;
using AuthenticationAuthorization.API.Models;
using AuthenticationAuthorization.Contract.User;
using AuthenticationAuthorization.Core.Users;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AuthenticationAuthorization.API.AuthenticationProvider
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            try
            {

                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                var token = new RefreshToken()
                {
                    Id = HashGenerator.GetHash(refreshTokenId),
                    ClientId = clientid,
                    Subject = context.Ticket.Identity.Name,
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
                context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

                token.ProtectedTicket = context.SerializeTicket();

                var result = await GetUserDbService().AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            //var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", allowedOrigin.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            string hashedTokenId = HashGenerator.GetHash(context.Token);

            try
            {

                var refreshToken = await GetUserDbService().FindRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class                    
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    var result = await GetUserDbService().RemoveRefreshToken(hashedTokenId);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Private Methods

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
        #endregion
    }
}