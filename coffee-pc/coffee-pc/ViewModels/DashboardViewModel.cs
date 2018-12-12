using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace coffee_pc.ViewModels
{
    class DashboardViewModel : Screen
    {
        private Visibility _closeVisible = Visibility.Collapsed;
        private Visibility _openVisible;

        public Visibility CloseVisible
        {

            get { return _closeVisible; }
            set
            {
                _closeVisible = value;
                NotifyOfPropertyChange(() => CloseVisible);
            }
        }

        public Visibility OpenVisible
        {
            get { return _openVisible; }
            set
            {
                _openVisible = value;
                NotifyOfPropertyChange(() => OpenVisible);
            }
        }


        public void ButtonClose() {
            CloseVisible = Visibility.Collapsed;
            OpenVisible = Visibility.Visible;

        }
        public void ButtonOpen() {
            OpenVisible = Visibility.Collapsed;
            CloseVisible = Visibility.Visible;
        }

        public void ActivateLogin()
        {
            var parentConductor = (Conductor<object>)(Parent);
            parentConductor.ActivateItem(new LoginViewModel());
        }

        public void ClearToken() {
            Properties.Settings.Default.token = "";
            Properties.Settings.Default.Save();
            ActivateLogin();
        }


    }
}
