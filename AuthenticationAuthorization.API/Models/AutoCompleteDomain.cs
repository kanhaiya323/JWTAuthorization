using AuthenticationAuthorization.API.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthenticationAuthorization.API.Models
{
    public class AutoCompleteDomain
    {
        public int Id { get; set; }
        public string Cd { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ParentCd { get; set; }
        public string label { get; set; }
        public string value { get; set; }
        public string type { get; set; }
    }

    public class AutoCompleteResponse
    {
       public List<AutoCompleteDomain> autoCompleteResults;
       public AutoCompleteResponse()
        {
            autoCompleteResults = new List<AutoCompleteDomain>();
        }
    }
}