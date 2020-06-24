using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthenticationAuthorization.API.Models
{
    public class RefreshTokenRequestParams
    {
        public string TokenId { get; set; }
    }
}