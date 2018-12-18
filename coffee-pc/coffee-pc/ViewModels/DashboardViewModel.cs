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
        private bool _isDialogOpen;
        private TableWatcher watcher = new TableWatcher();

        public DashboardViewModel()
        {
            GetOrders();
            var client = new SignalRMasterClient("http://localhost:5819/signalr",()=>GetOrders());
        }

        public async Task GetOrders()
        {
            System.Diagnostics.Debug.WriteLine("BuildViewModelAsync");
            //IsDialogOpen = true;
            List<OrdersResponseModel> Orders = await dashboardRepo.GetOrders();
            _orders.Clear();
            foreach (var o in Orders)
            {
                _orders.Add(o);
            }
            //IsDialogOpen = false;
        }

        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => Set(ref _isDialogOpen, value);
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
            var res = await dashboardRepo.FinalizeOrder(SelectedOrder.id, "completed");
            if (!res) Toast.ProvideToast().ShowError("Can't finalize order");
            }
        }

        public async Task RefuseOrder() {
            if (SelectedOrder != null)
            {
                var res = await dashboardRepo.FinalizeOrder(SelectedOrder.id, "refused");
                if (!res) Toast.ProvideToast().ShowError("Can't refuse order");
            }
            }



    }
}
