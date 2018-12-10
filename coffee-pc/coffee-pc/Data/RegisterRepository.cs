using coffee_pc.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_pc.Data
{
    class RegisterRepository
    {


        MainService ms = new MainService();

        public async Task<String> Register(String email, String password, String confirmPassword)
        {

            String res = await ms.RequestRegisterAsync(email, password,confirmPassword);

            return res;


        }
    }
}
