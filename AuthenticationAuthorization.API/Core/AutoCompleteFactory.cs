using AuthenticationAuthorization.API.Contract;
using AuthenticationAuthorization.API.Helper;
using AuthenticationAuthorization.API.Models;
using AuthenticationAuthorization.API.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AuthenticationAuthorization.API.Core
{
    public class AutoCompleteFactory : IAutoCompleteFactory
    {
        SqlConnection sqlConnection;
        AutoCompleteResponse IAutoCompleteFactory.AutoCompleteResult(AutoCompleteDomain autoCompleteDomain)
        {
            AutoCompleteResponse autoCompleteResponse = new AutoCompleteResponse();
            try
            {


                using (sqlConnection = new SqlConnection(DbConnection.connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("[dbo].[AutoComplete]", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    sqlCommand.Parameters.AddRange(new[]
                    {
                        new SqlParameter("@Id",autoCompleteDomain.Id),
                        new SqlParameter("@Value",autoCompleteDomain.value),
                        new SqlParameter("@Label",autoCompleteDomain.label),
                        new SqlParameter("@ParentCd",autoCompleteDomain.ParentCd),
                        new SqlParameter("@Type",autoCompleteDomain.type),
                });
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                    DataSet dataSet = new DataSet();
                    sqlDataAdapter.SelectCommand = sqlCommand;
                    sqlDataAdapter.Fill(dataSet);
                    if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                        {
                            autoCompleteResponse.autoCompleteResults.Add(new AutoCompleteDomain()
                            {
                                Id = Convert.ToInt32(dataRow["Id"]),
                                Name = autoCompleteDomain.Name,
                                Desc = autoCompleteDomain.Desc,
                                label = Convert.ToString(dataRow["label"]),
                                Cd = Convert.ToString(dataRow["Cd"]),
                                value = Convert.ToString(dataRow["value"]),
                                type = autoCompleteDomain.type,
                                ParentCd = autoCompleteDomain.ParentCd,
                            });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return autoCompleteResponse;

        }

    }
}
