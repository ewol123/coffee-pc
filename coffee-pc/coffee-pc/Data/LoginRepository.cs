﻿using coffee_pc.Models;
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

        public async Task<LoginResponse> Login(String email, String password) {

            LoginResponse res = await ms.RequestLoginAsync(email, password);

            return res;


        }


    }
}
