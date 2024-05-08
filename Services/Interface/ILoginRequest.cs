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
        string RoleAssigned(DTOLoginRequest loginRequest);
    }
}
