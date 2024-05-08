using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LoginRequest
    {
         public string UserName { get; set; }
         public string PassWord { get; set; }
         public string Role { get; set; }

        public LoginRequest() { 
        }

        public LoginRequest(DTOLoginRequest loginRequest)
        {
            this.UserName = loginRequest.UserName;
            this.PassWord = loginRequest.PassWord;
        }
    }
}