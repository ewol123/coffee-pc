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
        private Visibility _roleBtnsVisible = Visibility.Collapsed;
        private bool _isSelectRoleOpen = false;
        private BindableCollection<OrdersResponseModel> _orders = new BindableCollection<OrdersResponseModel>();
        private BindableCollection<UsersResponseModel> _users = new BindableCollection<UsersResponseModel>();
        private BindableCollection<RoleResponseModel> _roles = new BindableCollection<RoleResponseModel>();
        private OrdersResponseModel _selectedOrder { get; set; }
        private UsersResponseModel _selectedUser { get; set; }
        private DashboardRepository dashboardRepo = new DashboardRepository();
        private Visibility _ordersVisible { get; set; }
        private Visibility _adminGridVisible { get; set; }
        private string _progressBarVal { get; set; }
        private string _selectedRole { get; set; }

        public DashboardViewModel()
        {
            AdminGridVisible = Visibility.Collapsed;
            ProgressBarVal = "100";
            GetOrders();
            var client = new SignalRMasterClient("http://localhost:5819/signalr", () => GetOrders());
            OrdersVisible = Visibility.Visible;
        }



        public async Task OpenManagement() {
            OrdersVisible = Visibility.Collapsed;
            AdminGridVisible = Visibility.Visible;
           await GetUsers();
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


        public async Task GetUsers()
        {
            try { 
            ProgressBarVal = "0";
            List<UsersResponseModel> Users = await dashboardRepo.GetUsers();
            UserList.Clear();
            foreach (var u in Users)
            {
                UserList.Add(u);
            }
            ProgressBarVal = "100";
            }
            catch (Exception e) {

                ProgressBarVal = "100";
                Toast.ProvideToast().ShowError("Can't load users");
            }
        }

        public async Task GetRoles()
        {
            try
            {
                List<RoleResponseModel> Roles = await dashboardRepo.GetRoles();
                RoleList.Clear();

                foreach(var r in Roles)
                { 
                    RoleList.Add(r);
                }

            }
            catch (Exception e)
            {
                Toast.ProvideToast().ShowError("Can't load roles");
            }
        }

        public string SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                NotifyOfPropertyChange(() => SelectedRole);
            }
        }



        public string ProgressBarVal
        {
            get { return _progressBarVal; }
            set {
                _progressBarVal = value;
                NotifyOfPropertyChange(() => ProgressBarVal);
            }
        }


        public Visibility RoleBtnsVisible
        {
            get { return _roleBtnsVisible; }
            set
            {
                _roleBtnsVisible = value;
                NotifyOfPropertyChange(() => RoleBtnsVisible);
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

        public UsersResponseModel SelectedUser {

            get { return _selectedUser; }

            set
            {
                _selectedUser = value;
                NotifyOfPropertyChange(() => SelectedUser);
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

        public bool IsSelectRoleOpen
        {
            get { return _isSelectRoleOpen; }
            set
            {
                _isSelectRoleOpen = value;
                NotifyOfPropertyChange(() => IsSelectRoleOpen);
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

        public BindableCollection<UsersResponseModel> UserList
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => UserList);
            }
        }


        public BindableCollection<RoleResponseModel> RoleList
        { 
            get { return _roles; }
            set
            {
                _roles = value;
                NotifyOfPropertyChange(() => RoleList);
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

        public async Task SaveRole() {
            try
            {
                var res = await dashboardRepo.ManageUsersInRole(new RoleBindingModel {
                    Id = SelectedRole,
                    EnrolledUsers = new List<string> {SelectedUser.Id},
                    RemovedUsers = new List<string>() });

                if (!res) Toast.ProvideToast().ShowError("Can't add to role");
                else {
                    await GetUsers();
                    IsSelectRoleOpen = false;
                }
            }
            catch (Exception e)
            {
                Toast.ProvideToast().ShowError("Can't add to role");
            }
        }


        public async Task RemoveRole()
        {
            try
            {
                var res = await dashboardRepo.ManageUsersInRole(new RoleBindingModel
                {
                    Id = SelectedRole,
                    EnrolledUsers = new List<string>(),
                    RemovedUsers = new List<string> { SelectedUser.Id }
                });

                if (!res) Toast.ProvideToast().ShowError("Can't delete from role");
                else
                {
                    await GetUsers();
                    IsSelectRoleOpen = false;
                }
            }
            catch (Exception e)
            {
                Toast.ProvideToast().ShowError("Can't delete from role");
            }
        }

        public async Task CancelRole() {
                IsSelectRoleOpen = false;
        }

        public async Task OpenRoleSelector() {
            IsSelectRoleOpen = true;
            await GetRoles();
        }

   


    }
}
