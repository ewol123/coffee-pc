using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using coffee_pc.Models;
using Flurl;
using Flurl.Http;

namespace coffee_pc.Service
{
    public class MainService
    {
        const String BASE_URL = "http://localhost:5819";
        String token = Properties.Settings.Default.token;
        String clientId = Properties.Settings.Default.clientId;
        public async Task<LoginResponseModel> RequestLoginAsync(String email, String password)
        {
            try
            {
                LoginResponseModel res = await BASE_URL
               .AppendPathSegment("/oauth2/token")
               .WithHeaders(new { Accept = "application/json", Content_Type = "application/x-www-form-urlencoded" }).PostUrlEncodedAsync(new
               {
                   username = email,
                   password = password,
                   grant_type = "password",
                   client_id = clientId
               }).ReceiveJson<LoginResponseModel>();

                return res;
            }
            catch (FlurlHttpException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }


        public async Task<bool> RequestRegisterAsync(String email, String password, String confirmPassword)
        {
            try
            {

                var res = await BASE_URL
               .AppendPathSegment("/api/accounts/create")
               .WithHeaders(new { Accept = "application/json", Content_Type = "application/json" }).PostJsonAsync(new
               {
                   Email = email,
                   Password = password,
                   ConfirmPassword = confirmPassword
               });
                
                return res.IsSuccessStatusCode;
            }
            catch (FlurlHttpException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<OrdersResponseModel>> GetPlacedOrders() {
            try
            {
                System.Diagnostics.Debug.WriteLine(token);
                var res = await BASE_URL
                    .AppendPathSegment("/api/orders/placedOrders")
                    .WithOAuthBearerToken(token)
                    .GetAsync()
                    .ReceiveJson<List<OrdersResponseModel>>(); ;

                System.Diagnostics.Debug.WriteLine("response:", res);
                return res;
             }

            catch (FlurlHttpException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }


        public async Task<bool> FinalizeOrder(int id, string status) {
            try
            {
                var res = await BASE_URL
                    .AppendPathSegment("/api/orders/finalizeOrder")
                    .WithOAuthBearerToken(token)
                    .PutJsonAsync(new
                    {
                        Id = id,
                        Status = status,
                    });

                return res.IsSuccessStatusCode;

            }
            catch (FlurlHttpException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<UsersResponseModel>> GetUsers()
        {
            try
            {
                var res = await BASE_URL
                    .AppendPathSegment("/api/accounts/users")
                    .WithOAuthBearerToken(token)
                    .GetAsync()
                    .ReceiveJson<List<UsersResponseModel>>(); ;

                return res;
            }

            catch (FlurlHttpException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }



        public async Task<List<RoleResponseModel>> GetRoles()
        {
            try
            {
                var res = await BASE_URL
                    .AppendPathSegment("/api/roles")
                    .WithOAuthBearerToken(token)
                    .GetAsync()
                    .ReceiveJson<List<RoleResponseModel>>(); ;
                return res;
            }

            catch (FlurlHttpException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }


        public async Task<bool> ManageUsersInRole(RoleBindingModel model)
        {
            try
            {
                var res = await BASE_URL
                    .AppendPathSegment("/api/roles/ManageUsersInRole")
                    .WithOAuthBearerToken(token)
                    .PostJsonAsync(model);

                return res.IsSuccessStatusCode;

            }
            catch (FlurlHttpException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }
        }


    }
}
