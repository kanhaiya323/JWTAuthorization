using AuthenticationAuthorization.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.API.Contract
{
    public interface IAutoCompleteFactory
    {
        AutoCompleteResponse AutoCompleteResult(AutoCompleteDomain autoCompleteDomain);
    }
}
