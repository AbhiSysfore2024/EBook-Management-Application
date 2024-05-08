using AppSettings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Models;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LoginRequestStoredProcedure : ILoginRequest
    {
        private readonly ConnectionStrings _connection;

        public LoginRequestStoredProcedure(IOptions<ConnectionStrings> connection)
        {
            _connection = connection.Value;
        }

        public string RoleAssigned(DTOLoginRequest loginRequest)
        {
            string role = "";
            using (SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio))
            {
                try
                {
                    connection.Open();
                    string returnRoleQuery = "GetRole";
                    SqlCommand command = new SqlCommand(returnRoleQuery, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Username", SqlDbType.VarChar).Value = loginRequest.UserName;
                    command.Parameters.Add("@Password", SqlDbType.VarChar).Value = loginRequest.PassWord;

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        role = Convert.ToString(result);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return role;
        }

    }
}
