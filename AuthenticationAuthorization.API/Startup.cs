using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using AuthenticationAuthorization.API.AuthenticationProvider;
using AuthenticationAuthorization.API.TokenFormat;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(AuthenticationAuthorization.API.Startup))]

namespace AuthenticationAuthorization.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            ConfigureOAuth(app);
            ConfigureOAuthTokenConsumption(app);

            HttpConfiguration config = new HttpConfiguration();
            app.UseWebApi(config);
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = ConfigurationManager.AppSettings["AllowInsecureHttp"] == "1",
                TokenEndpointPath = new PathString("/oauth2/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiryInMinutes"])),
                Provider = new AuthProvider(),
                AccessTokenFormat = new JWTFormats(ConfigurationManager.AppSettings["as:TokenIssuer"]),
                RefreshTokenProvider = new RefreshTokenProvider()
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }


        public void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = ConfigurationManager.AppSettings["as:TokenIssuer"];
            var client = ConfigurationManager.AppSettings["as:ClientId"];
            byte[] secret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:ClientSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { client },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                    },
                    Provider = new OAuthBearerAuthenticationProvider
                    {
                        OnValidateIdentity = context =>
                        {
                            context.Ticket.Identity.AddClaim(new System.Security.Claims.Claim("newCustomClaim", "AnyCustomeValue"));
                            context.Ticket.Identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.MobilePhone, "xxxxxx8010"));
                            return Task.FromResult<object>(null);
                        }
                    }
                });
        }
    }
}
