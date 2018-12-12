using Caliburn.Micro;
using coffee_pc.Data;
using coffee_pc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Messages;

namespace coffee_pc.ViewModels
{
    class RegisterViewModel: Screen
    {
        Notifier notify;
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
            notify = Toast.ProvideToast();
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
           

            if (Password == null) {
                notify.ShowWarning("Password can't be empty");
                return null;
            }

            if (Password != ConfirmPassword) {
                notify.ShowWarning("Passwords don't match");
                return null;
            }

            if (Email == null) {
                notify.ShowWarning("Email can't be empty");
                return null;
            }

            try {
                var addr = new System.Net.Mail.MailAddress(Email);
            }
            catch (Exception e ) {
                notify.ShowWarning("Email is not valid");
                return null;
            }

            IsRegisterDialogOpen = true;
            bool response = await registerRepo.Register(Email, Password, ConfirmPassword);
            IsRegisterDialogOpen = false;
            if (!response)
            {
                notify.ShowError("Error, please try again");
            }
            else {
                ActivateLogin();
                notify.ShowSuccess("Registration successful, please confirm email");
            }
           
            return null;
        }

    }
}
