using AuthenticationAuthorization.API.Contract;
using AuthenticationAuthorization.API.Helper;
using AuthenticationAuthorization.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AuthenticationAuthorization.API.Core
{
    public class SuffixService : ISuffixService
    {
        SqlConnection sqlConnection;
        List<Suffix> ISuffixService.GetSuffix()
        {

            var suffix = new List<Suffix>();
            using (sqlConnection = new SqlConnection(DbConnection.connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("[dbo].[GetSuffix]", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(dataSet);
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                    {
                        suffix.Add(new Suffix()
                        {

                            Id = Convert.ToInt64(dataRow["Id"]),
                            Name = Convert.ToString(dataRow["Name"]),
                            Description = Convert.ToString(dataRow["Description"])
                        });
                    }
                }

            }
            return suffix;
        }
    }
}