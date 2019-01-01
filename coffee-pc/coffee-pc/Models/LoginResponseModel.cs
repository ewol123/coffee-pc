using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_pc.Models
{
    public class LoginResponseModel
    {
        public String access_token { get; set; }
        public String token_type { get; set; }  
        public int expires_in { get; set; }
    }
}
