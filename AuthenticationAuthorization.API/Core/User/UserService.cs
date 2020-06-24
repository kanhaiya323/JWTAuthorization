using AuthenticationAuthorization.API.Contract.User;
using AuthenticationAuthorization.API.Helper;
using AuthenticationAuthorization.API.Response;
using AuthenticationAuthorization.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AuthenticationAuthorization.API.Core.User
{
    public class UserService : IUserService
    {
        SqlConnection sqlConnection;
        ApiResponse apiResponse = new ApiResponse();

        int IUserService.CheckEmail([FromUri] string inputEmail)
        {
            try
            {
                using(sqlConnection = new SqlConnection(DbConnection.connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("[dbo].[GetEmail]", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddRange(new[]
                    {
                        new SqlParameter("@inputEmail",inputEmail)
                    });
                    var isDuplicate = sqlCommand.ExecuteScalar();
                    return (isDuplicate != null) ? 1 : 0;
                };
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        ApiResponse IUserService.RegistreUser(UserDomain user)
        {
            try
            {
                using (sqlConnection = new SqlConnection(DbConnection.connectionString))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new SqlCommand("[dbo].[AddUser]", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddRange(new[]
                    {
                    new SqlParameter("@Suffix",user.Suffix),
                    new SqlParameter("@FirstName",user.FirstName),
                    new SqlParameter("@MiddleName",user.MiddleName),
                    new SqlParameter("@LastName",user.LastName),
                    new SqlParameter("@FullName",user.FullName),
                    new SqlParameter("@Email",user.Email),
                    new SqlParameter("@Phone",user.Phone),
                    new SqlParameter("@Mobile",user.Mobile),
                    new SqlParameter("@Password",user.Password),
                    new SqlParameter("@EffectiveDt",user.EffectiveDt),
                    new SqlParameter("@ExpiryDt",user.ExpiryDt)
                });

                    var rowEffeted = sqlCommand.ExecuteNonQuery();
                    if (rowEffeted > 0)
                    {
                        apiResponse.Status = true;
                        apiResponse.Message = "Registration is done.";
                    }
                    else
                    {
                        apiResponse.Status = false;
                        apiResponse.Message = "Sorry Please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                apiResponse.Status = false;
                apiResponse.Message = "[Exception]: " + ex;
            }
            return apiResponse;
        }
    }
}