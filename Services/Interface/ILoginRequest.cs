using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ILoginRequest
    {
        string Signup(DTOLoginRequest loginRequest);
        string RoleAssigned(DTOLoginRequest loginRequest);
        string GenerateJwtToken(DTOLoginRequest loginDTO, string role);
        List<object> GetAllUsers();
        string ResetPassword(string username, string newPassword);
    }
}
