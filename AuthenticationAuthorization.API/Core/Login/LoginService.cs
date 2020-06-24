
using AuthenticationAuthorization.API.Helper;
using AuthenticationAuthorization.API.Models;
using AuthenticationAuthorization.Contract.User;
using AuthenticationAuthorization.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AuthenticationAuthorization.Core.Users
{
    public class LoginService : ILoginService
    {
        SqlConnection sqlConnection;
        /// <summary>
        /// Function for Adding  Refresh Token
        /// </summary>
        /// <param name="refreshToken">RefreshToken</param>
        /// <returns>List of Organization</returns>
        public async Task<bool> AddRefreshToken(RefreshToken refreshToken)
        {
            using (sqlConnection = new SqlConnection(DbConnection.connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("[dbo].[AddRefreshTokens]", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddRange(new[]
                {
                           new SqlParameter(){ParameterName="@TokenId",Value = refreshToken.Id},
                           new SqlParameter() { ParameterName = "@Subject", Value = refreshToken.Subject},
                           new SqlParameter() { ParameterName = "@ClientId", Value = refreshToken.ClientId},
                           new SqlParameter() { ParameterName = "@IssuedUtc ", Value = refreshToken.IssuedUtc},
                           new SqlParameter() { ParameterName = "@ExpiresUtc ", Value = refreshToken.ExpiresUtc},
                           new SqlParameter() { ParameterName = "@ProtectedTicket", Value = refreshToken.ProtectedTicket}

                });
                var rowEffeted = sqlCommand.ExecuteNonQuery();

               return await Task.FromResult(rowEffeted > 0);
            }             

        }

        /// <summary>
        /// Function for Removing  Token
        /// </summary>
        /// <param name="hashedTokenId"></param>
        /// <returns>Status</returns>
        public async Task<bool> RemoveRefreshToken(string hashedTokenId)
        {
            using(sqlConnection = new SqlConnection(DbConnection.connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("[dbo].[RemoveRefreshTokens]", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddRange(new[] {
                    new SqlParameter("@refreshTokenId ", hashedTokenId),
                });
                var rowEffeted = sqlCommand.ExecuteNonQuery();
                return await Task.FromResult(rowEffeted > 0);
            }          
            
        }


        /// <summary>
        /// Function for Finding  Refresh Token
        /// </summary>
        /// <param name="hashedTokenId"></param>
        /// <returns>Token</returns>

        public async Task<RefreshToken> FindRefreshToken(string hashedTokenId)
        {
            using (sqlConnection = new SqlConnection(DbConnection.connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("[dbo].[FindRefreshTokens]", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddRange(new[]
                {
                    new SqlParameter("@refreshTokenId ", hashedTokenId),
                });
                DataSet dataSet = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.Fill(dataSet);

            RefreshToken refreshToken = null;
            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {

                    refreshToken =
                          new RefreshToken()
                          {
                              Id = Convert.ToString(dataRow["TokenId"]),
                              Subject = Convert.ToString(dataRow["Subject"]),
                              ClientId = Convert.ToString(dataRow["ClientId"]),
                              IssuedUtc = Convert.ToDateTime(dataRow["IssuedUtc"]),
                              ExpiresUtc = Convert.ToDateTime(dataRow["ExpiresUtc"]),
                              ProtectedTicket = Convert.ToString(dataRow["ProtectedTicket"])
                          };
                }
            }
            return await Task.FromResult(refreshToken);
        }
        }


        UserDomain ILoginService.FindUser(string Email = "kanhaiya", string Password = "12345678")
        {
            UserDomain userDetails = null;
            Password = HashGenerator.GetHash(Password);

            using (sqlConnection = new SqlConnection(DbConnection.connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand("[dbo].[FindUser]", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                sqlCommand.Parameters.AddRange(new[]
                {
                           new SqlParameter("@Email", Email),
                           new SqlParameter("@Password", Password),
                });
                DataSet dataSet = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = sqlCommand;                
                sda.Fill(dataSet);
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dtrow in dataSet.Tables[0].Rows)
                    {
                        userDetails = new UserDomain()
                        {
                            Id = Convert.ToInt64(dtrow["Id"]),
                            //UserName = Convert.ToString(dtrow["UserName"]),
                            Email = Convert.ToString(dtrow["Email"]),
                            FullName = Convert.ToString(dtrow["FullName"]),
                            EffectiveDt = Convert.ToDateTime(dtrow["EffectiveDt"]),
                            ExpiryDt = Convert.ToDateTime(dtrow["ExpiryDt"]),
                            IsActive = Convert.ToBoolean(dtrow["IsActive"])
                        };

                    }
                }

            }
            //User user = new User()
            //{
            //    UserName = Email,
            //    Password = Password
            //};

            return userDetails;
        }
    }
}
