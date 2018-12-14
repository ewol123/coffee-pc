using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_pc.Models
{
    class OrdersModel
    {
        public int TableNum { get; set; }
        public string Payed { get; set; }
        public string PaymentMethod { get; set; }
        public BindableCollection<OrderedProductsModel> Products { get; set; }
        public string Price { get; set; }
    }
}
