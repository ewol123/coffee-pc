using Caliburn.Micro;
using coffee_pc.Data;
using coffee_pc.Models;
using coffee_pc.Services;
using coffee_pc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToastNotifications.Messages;

namespace coffee_pc.ViewModels
{
    class DashboardViewModel : Screen

    {
        private Visibility _closeVisible = Visibility.Collapsed;
        private Visibility _openVisible;
        private BindableCollection<OrdersResponseModel> _orders = new BindableCollection<OrdersResponseModel>();
        private OrdersResponseModel _selectedOrder;
        private DashboardRepository dashboardRepo = new DashboardRepository();
        private Visibility _ordersVisible { get; set; }
        private Visibility _adminGridVisible { get; set; }
        private string _progressBarVal { get; set; }
        public DashboardViewModel()
        {
            AdminGridVisible = Visibility.Collapsed;
            ProgressBarVal = "100";
            GetOrders();
            var client = new SignalRMasterClient("http://localhost:5819/signalr", () => GetOrders());

            OrdersVisible = Visibility.Visible;
        }



        public void OpenManagement() {
            OrdersVisible = Visibility.Collapsed;
            AdminGridVisible = Visibility.Visible;
        }

        public void OpenOrders() {
            AdminGridVisible = Visibility.Collapsed;
            OrdersVisible = Visibility.Visible;
        }

        public async Task GetOrders()
        {
            System.Diagnostics.Debug.WriteLine("BuildViewModelAsync");
            ProgressBarVal = "0";
            List<OrdersResponseModel> Orders = await dashboardRepo.GetOrders();
            OrderList.Clear();
            foreach (var o in Orders)
            {
                OrderList.Add(o);
            }
            ProgressBarVal = "100";
        }


        public string ProgressBarVal
        {
            get { return _progressBarVal; }
            set {
                _progressBarVal = value;
                NotifyOfPropertyChange(() => ProgressBarVal);
                }
        }

        public Visibility OrdersVisible
        {
            get { return _ordersVisible; }
            set {
                _ordersVisible = value;
                NotifyOfPropertyChange(() => OrdersVisible);
             }
        }

        public Visibility AdminGridVisible
        {
            get { return _adminGridVisible; }
            set
            {
                _adminGridVisible = value;
                NotifyOfPropertyChange(() => AdminGridVisible);
            }
        }


        public OrdersResponseModel SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;
                NotifyOfPropertyChange(() => SelectedOrder);
            }
        }


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

        public BindableCollection<OrdersResponseModel> OrderList
        {
            get { return _orders; }
            set
            {
                _orders = value;
                NotifyOfPropertyChange(() => OrderList);      
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

        public async Task FinalizeOrder()
        {
            if (SelectedOrder != null)
            {
                ProgressBarVal = "0";
                var res = await dashboardRepo.FinalizeOrder(SelectedOrder.id, "completed");
                ProgressBarVal = "100";
                if (!res) Toast.ProvideToast().ShowError("Can't finalize order");
            }
        }

        public async Task RefuseOrder() {
            if (SelectedOrder != null)
            {
                ProgressBarVal = "0";
                var res = await dashboardRepo.FinalizeOrder(SelectedOrder.id, "refused");
                ProgressBarVal = "100";
                if (!res) Toast.ProvideToast().ShowError("Can't refuse order");
            }
            }



    }
}
