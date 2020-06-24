using AuthenticationAuthorization.API.Models;
using AuthenticationAuthorization.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuthenticationAuthorization.API.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class LoginController : ApiController
    {
        /// <summary>
        /// Create new User Permission
        /// </summary>
        /// <param name="loginRequestParams">LoginRequestParams</param>
        /// <returns>Status</returns>
        // POST api/login 
        [AllowAnonymous]
        [Route("login")]
        public async Task<HttpResponseMessage> Post(LoginRequestPram loginRequestParams)
         {
            using (var httpclient = new HttpClient())
            {

                httpclient.BaseAddress = new Uri(ConfigurationManager.AppSettings["APIHostURL"]);
                httpclient.DefaultRequestHeaders.Accept.Clear();
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Dictionary<string, string> form = new Dictionary<string, string>
               {
                   { "grant_type", "password" },
                   { "userName", loginRequestParams.Email },
                   { "password", loginRequestParams.Password },
                   { "client_id",ConfigurationManager.AppSettings["as:ClientId"] },
                   { "client_secret",ConfigurationManager.AppSettings["as:ClientSecret"] },
               };

                return await httpclient.PostAsync("oauth2/token", new FormUrlEncodedContent(form));

            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("newtoken")]
        public async Task<HttpResponseMessage> GetRefreshToken(RefreshTokenRequestParams refreshTokenRequestParams)
        {
            using (var httpclient = new HttpClient())
            {

                httpclient.BaseAddress = new Uri(ConfigurationManager.AppSettings["APIHostURL"]);
                httpclient.DefaultRequestHeaders.Accept.Clear();
                httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Dictionary<string, string> form = new Dictionary<string, string>
               {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", refreshTokenRequestParams.TokenId },
                    { "client_id",ConfigurationManager.AppSettings["as:ClientId"] },
                    { "client_secret",ConfigurationManager.AppSettings["as:ClientSecret"] },
               };
                return await httpclient.PostAsync("oauth2/token", new FormUrlEncodedContent(form));
            }
        }
    }
}
