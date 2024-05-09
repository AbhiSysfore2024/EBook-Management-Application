﻿using AppSettings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services.Interface;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public string GenerateJwtToken(DTOLoginRequest loginDTO, string role)
        {
            try
            {
                if (string.IsNullOrEmpty(role))
                {
                    throw new ArgumentException("Invalid username or password");
                }
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyForAuthenticationOfApplication"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claim = new List<Claim>
            {
            new Claim(ClaimTypes.Name, loginDTO.UserName),
            new Claim(ClaimTypes.Role, role)
            };

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: "abhilash",
                    audience: "abhilash",
                    claims: claim,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signinCredentials
                );

                return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            }
            catch ( Exception e )
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }
    }
}

