using Caliburn.Micro;
using coffee_pc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_pc.ViewModels
{
    class RegisterViewModel: Screen
    {
        RegisterRepository registerRepo = new RegisterRepository();
        private string _email;
        private bool _IsRegisterDialogOpen;
        public bool IsRegisterDialogOpen
        {
            get => _IsRegisterDialogOpen;
            set => Set(ref _IsRegisterDialogOpen, value);
        }
        public string Password { private get; set; }
        public string ConfirmPassword { private get; set; }

        public RegisterViewModel()
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

        public void ActivateLogin() {
            var parentConductor = (Conductor<object>)(Parent);
            parentConductor.ActivateItem(new LoginViewModel());
        }


        public async Task<String> Register() {
            IsRegisterDialogOpen = true;
            if (Password != ConfirmPassword) {
                IsRegisterDialogOpen = false;
                return null;
            }
            var addr = new System.Net.Mail.MailAddress(Email);
            if (addr.Address != Email) {
                IsRegisterDialogOpen = false;
                return null;
            } 
            string response = await registerRepo.Register(Email, Password, ConfirmPassword);
            IsRegisterDialogOpen = false;
            System.Diagnostics.Debug.WriteLine(response);
            return response;
        }

    }
}
