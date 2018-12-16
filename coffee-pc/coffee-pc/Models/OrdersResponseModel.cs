using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_pc.Models
{
    class OrdersResponseModel
    {
        public int id { get; set; }
        public int tableNum { get; set; }
        public string payed { get; set; }
        public string paymentMethod { get; set; }
        public BindableCollection<OrderedProductsResponseModel> products { get; set; }
        public string totalPrice { get; set; }
    }
}
