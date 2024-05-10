using AppSettings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services.Interface;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public string Signup(LoginRequest loginRequest)
        {
            using (SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(loginRequest.UserName) || string.IsNullOrWhiteSpace(loginRequest.PassWord))
                    {
                        throw new ArgumentException("Username or password cannot be empty or whitespace");
                    }

                    if (loginRequest.Role != "Admin" && loginRequest.Role != "User")
                    {
                        throw new ArgumentException("Role must be either 'Admin' or 'User'");
                    }

                    string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(loginRequest.PassWord, 13);
                    connection.Open();

                    string signupQuery = "SignupHash";
                    SqlCommand command = new SqlCommand(signupQuery, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = loginRequest.UserName;
                    command.Parameters.Add("@Password", SqlDbType.VarChar).Value = passwordHash;
                    command.Parameters.Add("@RoleAssigned", SqlDbType.VarChar).Value = loginRequest.Role;

                    command.ExecuteNonQuery();

                    return "User successfully registered";
                }
                catch (Exception ex)
                {
            
                    Console.WriteLine("Error: " + ex.Message);
                    return ex.Message;
                }
            }
        }


        public string RoleAssigned(DTOLoginRequest loginRequest)
        {
            string role = "";
            using SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio);
            try
            {
            connection.Open();
            string hashedPassword = "";
            string loginQuery = "LoginHash";

            SqlCommand command = new SqlCommand(loginQuery, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = loginRequest.UserName;

            hashedPassword = command.ExecuteScalar() as string;

                if (hashedPassword != null && BCrypt.Net.BCrypt.EnhancedVerify(loginRequest.PassWord, hashedPassword))
                {
                    string roleQuery = "GetRole";
                     SqlCommand roleCommand = new SqlCommand(roleQuery, connection);
                    roleCommand.CommandType = CommandType.StoredProcedure;

                    roleCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = loginRequest.UserName;
                    roleCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = hashedPassword;

                     var result = roleCommand.ExecuteScalar();
                     if (result != null)
                     {
                          role = Convert.ToString(result);
                     }
                }
                else
                {
                    Console.WriteLine("Invalid username or password");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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

