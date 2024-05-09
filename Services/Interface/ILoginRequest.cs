using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ILoginRequest
    {
        string Signup(LoginRequest loginRequest);
        string RoleAssigned(DTOLoginRequest loginRequest);
        string GenerateJwtToken(DTOLoginRequest loginDTO, string role);
    }
}
