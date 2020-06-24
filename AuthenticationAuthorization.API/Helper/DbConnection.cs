using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AuthenticationAuthorization.API.Helper
{
    public  class DbConnection
    {
       public static string connectionString = ConfigurationManager.ConnectionStrings["myDbConnection"].ConnectionString;
    }

 
}