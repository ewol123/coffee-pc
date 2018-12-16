using Caliburn.Micro;
using coffee_pc.Models;
using coffee_pc.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_pc.Data
{
    class DashboardRepository
    {
        MainService ms = new MainService();

        public async Task<List<OrdersResponseModel>> GetOrders()
        {
            var res = await ms.GetPlacedOrders();
          
            return res;

        }
    }
}
