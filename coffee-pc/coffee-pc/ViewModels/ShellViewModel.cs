using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace coffee_pc.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        public ShellViewModel()
        {
            String token = Properties.Settings.Default.token;
            if (token == "")
                ActivateItem(new LoginViewModel());
            else
                ActivateItem(new DashboardViewModel());
        }

        public void close_program() {
            TryClose();
        }

        public void minimize_program() {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }


        public void maximize_program()
        {
            if (App.Current.MainWindow.WindowState == WindowState.Maximized)
                App.Current.MainWindow.WindowState = WindowState.Normal;

            else App.Current.MainWindow.WindowState = WindowState.Maximized;
        }
       
        public void drag() {
            try
            {
                App.Current.MainWindow.DragMove();
            }
            catch (Exception e)
            {
                
            }
            }




    }
}
