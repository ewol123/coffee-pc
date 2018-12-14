using Caliburn.Micro;
using coffee_pc.Models;
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
        private BindableCollection<OrdersModel> _orders = new BindableCollection<OrdersModel>();
        private OrdersModel _selectedOrder;


        public DashboardViewModel()
        {
            OrderList.Add(new OrdersModel { TableNum=1,Payed="Payed",PaymentMethod="PayPal", Price="$25", Products = new BindableCollection<OrderedProductsModel> { new OrderedProductsModel { Name = "Americano", Quantity = 3, Price = "$5" } } });
            OrderList.Add(new OrdersModel { TableNum = 11, Payed = "Not Payed", PaymentMethod = "Cash", Price = "$15", Products = new BindableCollection<OrderedProductsModel> { new OrderedProductsModel { Name = "Americano", Quantity = 3, Price = "$5" } } });
        }


        public OrdersModel SelectedOrder
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

        public BindableCollection<OrdersModel> OrderList
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

        public void CompletedBtn() {

            if (SelectedOrder == null) return;

            OrderList.Remove(OrderList.Where(ol => ol.TableNum == SelectedOrder.TableNum).Single());
        }

        public void RefusedBtn() {
            if (SelectedOrder == null) return;

            OrderList.Remove(OrderList.Where(ol => ol.TableNum == SelectedOrder.TableNum).Single());


        }


        public void DummyDataAdd() {

            var vmi = new BindableCollection<OrderedProductsModel> {
                new OrderedProductsModel { Name = "Americano", Quantity = 3, Price = "$5" },
                new OrderedProductsModel { Name = "Café affogato", Quantity = 2, Price = "$6" },
                new OrderedProductsModel { Name = "Espresso", Quantity = 1, Price = "$3" },
                new OrderedProductsModel { Name = "Ristretto", Quantity = 4, Price = "$4" }
            };

            OrderList.Add(new OrdersModel { TableNum = 9, Payed = "Not Payed", PaymentMethod = "Cash", Price = "$15", Products = vmi });
        }


    }
}
