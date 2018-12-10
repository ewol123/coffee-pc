using Caliburn.Micro;
using coffee_pc.Data;
using coffee_pc.Models;
using coffee_pc.Utils;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications.Messages;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace coffee_pc.ViewModels
{
    class LoginViewModel : Screen
    {
        LoginRepository loginRepo = new LoginRepository();
        private string _email;
        public string Password { private get; set; }
        private bool _IsDialogOpen;
        public bool IsDialogOpen
        {
            get => _IsDialogOpen;
            set => Set(ref _IsDialogOpen, value);
        }

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


        public void ActivateRegister() {
            var parentConductor = (Conductor<object>)(Parent);
            parentConductor.ActivateItem(new RegisterViewModel());
            
        }

        public async Task Login() {
            IsDialogOpen = true;
            LoginResponse response = await loginRepo.Login(Email,Password);
            IsDialogOpen = false;
            System.Diagnostics.Debug.WriteLine(response.access_token);
        }
    }
}
