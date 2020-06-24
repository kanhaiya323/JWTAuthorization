using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System.Configuration;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens;

namespace AuthenticationAuthorization.API.TokenFormat
{
    public class JWTFormats : ISecureDataFormat<AuthenticationTicket>
    {
        private const string ClientPropertyKey = "as:client_id";

        private readonly string _issuer = string.Empty;
        public JWTFormats(string issuer)
         {
            _issuer = issuer;
        }
        public string Protect(AuthenticationTicket data)
        {
            var clientId = ConfigurationManager.AppSettings["as:ClientId"];
            var secret = ConfigurationManager.AppSettings["as:ClientSecret"];

            var keyByteArray = TextEncodings.Base64Url.Decode(secret);
            var signingKey = new HmacSigningCredentials(keyByteArray);

            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            var token = new JwtSecurityToken(_issuer, clientId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);
            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new System.NotImplementedException();
        }
    }
}