using AuthenticationAuthorization.API.Contract;
using AuthenticationAuthorization.API.Core;
using AuthenticationAuthorization.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AuthenticationAuthorization.API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api")]
    public class CommonController : ApiController
    {
        //Default Constructor
        public CommonController() { }

        [HttpPost]
        [Route("AutoComplete")]
        public IHttpActionResult AutoComplete(AutoCompleteDomain autoComplete)
        {
            try
            {
                return Ok(GetAutoCompleteService().AutoCompleteResult(autoComplete));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IAutoCompleteFactory GetAutoCompleteService()
        {
            IAutoCompleteFactory autoCompleteFactory = new AutoCompleteFactory();
            return autoCompleteFactory;
        }
    }
}
