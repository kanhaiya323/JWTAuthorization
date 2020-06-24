using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Web;

namespace AuthenticationAuthorization.API.Response
{
    public class ApiResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}