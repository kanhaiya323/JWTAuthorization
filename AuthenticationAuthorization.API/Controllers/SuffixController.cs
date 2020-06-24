using AuthenticationAuthorization.API.Contract;
using AuthenticationAuthorization.API.Core;
using AuthenticationAuthorization.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace AuthenticationAuthorization.API.Controllers
{
    [RoutePrefix("api")]
    public class SuffixController : ApiController
    {
        [Route("Suffix")]
        public IHttpActionResult Get()
        {
            return Ok(GetSuffixService().GetSuffix());
        }

        //Service call
        public ISuffixService GetSuffixService()
        {
            ISuffixService suffixService = new SuffixService();
            return suffixService;
        }
    }
}
