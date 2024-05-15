using AppSettings;
using Exceptions;
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
        private readonly JWTClaimDetails _jwtDetails;

        public LoginRequestStoredProcedure(IOptions<ConnectionStrings> connection, IOptions<JWTClaimDetails> jwtDetails)
        {
            _connection = connection.Value;
            _jwtDetails = jwtDetails.Value;
        }

        public string Signup(DTOLoginRequest loginRequest)
        {
            using (SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(loginRequest.UserName) || string.IsNullOrWhiteSpace(loginRequest.PassWord))
                    {
                        throw new EmptOrNullSpaceUsrnmePsswrd("Username or password cannot be empty or whitespace");
                    }

                    if (string.Equals(loginRequest.UserName, loginRequest.PassWord))
                    {
                        throw new EqualUserNameAndPassword("Username & password cannot be equal");
                    }

                    string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(loginRequest.PassWord, 13);
                    connection.Open();

                    string checkUserNameQuery = "UniqueUserName";
                    SqlCommand checkcommand = new SqlCommand(checkUserNameQuery, connection);
                    checkcommand.CommandType = CommandType.StoredProcedure;
                    checkcommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = loginRequest.UserName;
                    int uniqueUser = (int)checkcommand.ExecuteScalar();

                    if (uniqueUser > 0)
                    {
                        throw new NotUniqueUserName("This username is already taken. Try another");
                    }
                    string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(loginRequest.PassWord, 13);

                    string signupQuery = "SignupHash";
                    SqlCommand command = new SqlCommand(signupQuery, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = loginRequest.UserName;
                    command.Parameters.Add("@Password", SqlDbType.VarChar).Value = passwordHash;
                    command.Parameters.Add("@RoleAssigned", SqlDbType.VarChar).Value = Role.User;

                    command.ExecuteNonQuery();

                    return "User successfully registered";
                }
                catch (EmptOrNullSpaceUsrnmePsswrd ce)
                {
                    return ce.Message;
                }

                catch (Exception ex) when (ex is EmptOrNullSpaceUsrnmePsswrd ||
                                           ex is EqualUserNameAndPassword ||
                                           ex is NotUniqueUserName)
                {
                    return ex.Message;
                }

                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    return e.Message;
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
                    throw new InvalidUserNamePassword("Invalid username or password");
                }
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtDetails.Key));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claim = new List<Claim>
                {
                new Claim(ClaimTypes.Name, loginDTO.UserName),
                new Claim(ClaimTypes.Role, role)
                };

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jwtDetails.Issuer,
                    audience: _jwtDetails.Audience,
                    claims: claim,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signinCredentials
                );

                return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            }

            catch (InvalidUserNamePassword iup)
            {
                return iup.Message;
            }

            catch ( Exception e )
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }

        public List<object> GetAllUsers()
        {
            List<object> getAllUsers = new List<object> ();
            using SqlConnection connection = new SqlConnection(_connection.SQLServerManagementStudio);
            try
            {
                connection.Open();
                string getAllUsersQuery = "GetAllUsers";
                SqlCommand command = new SqlCommand(getAllUsersQuery, connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string username = reader.GetString(0);
                    string roleAssigned = reader.GetString(2);

                    var users = new 
                    {
                        UserName = username,
                        Role = (Role)Enum.Parse(typeof(Role), roleAssigned)
                    };

                    getAllUsers.Add(users);
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine(e.Message);
            }
            return getAllUsers;
        }

    }
}