using Caliburn.Micro;
using coffee_pc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace coffee_pc.ViewModels
{
    class LoginViewModel : Screen
    {
        LoginRepository loginRepo = new LoginRepository();
        private string _email;
        public string Password { private get; set; }
        public LoginViewModel()
        {
        }

        public string Email
        {

            get { return _email; }
            set
            {
                _email = value;
                NotifyOfPropertyChange(() => Email);
            }
        }

     


        public void Login() {
            var response = loginRepo.Login(Email,Password);
            System.Diagnostics.Debug.WriteLine(response);
        }

    }
}
