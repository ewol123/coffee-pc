using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace coffee_pc.ViewModels
{
    public class ShellViewModel : Screen
    {

        public String email { get; set; }
        public String password { get; set; }


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
           App.Current.MainWindow.DragMove();
        }

    }
}
