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

        public async Task<bool> FinalizeOrder(int id, string status)
        {
            var res = await ms.FinalizeOrder(id, status);
            return res;
        }

        public async Task<List<UsersResponseModel>> GetUsers()
        {
            var res = await ms.GetUsers();

            return res;
        }

        public async Task<List<RoleResponseModel>> GetRoles()
        {

            var res = await ms.GetRoles();

            return res;

        }

        public async Task<bool> ManageUsersInRole(RoleBindingModel model)
        {
            var res = await ms.ManageUsersInRole(model);

            return res;

        }

    }
}
