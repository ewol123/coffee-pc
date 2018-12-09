using coffee_pc.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
namespace coffee_pc.Data
{
    class LoginRepository
    {

        MainService ms = new MainService();

        public Task<String> Login(String email, String password) {

            var res = ms.RequestLoginAsync(email, password);

            return res;


        }


    }
}
