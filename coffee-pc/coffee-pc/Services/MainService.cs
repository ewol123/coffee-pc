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
using Polly;

namespace coffee_pc.Service
{
    public class MainService
    {
        const String RES_URL = "http://localhost:5819";
        const String AUTH_URL = "http://localhost:5821";
        static String clientId = Properties.Settings.Default.clientId;

        public Policy providePolicy() {
            var retryPolicy = Policy
                   .Handle<FlurlHttpException>()
                   .RetryAsync(1, onRetryAsync: async (exception, retryCount) =>
                   {
                       await RefreshToken();
                   });

            return retryPolicy;
        }

        public async Task<LoginResponseModel> RequestLoginAsync(String email, String password)
        {
            try
            {
                LoginResponseModel res = await AUTH_URL
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
                return null;
            }
        }

        public static  async Task<bool> RefreshToken()
        {
            try
            {
                var refreshToken = Properties.Settings.Default.refresh_token;
                LoginResponseModel res = await AUTH_URL
               .AppendPathSegment("/oauth2/token")
               .WithHeaders(new { Accept = "application/json", Content_Type = "application/x-www-form-urlencoded" }).PostUrlEncodedAsync(new
               {
                   refresh_token = refreshToken,
                   grant_type = "refresh_token",
                   client_id = clientId
               }).ReceiveJson<LoginResponseModel>();


                if (res == null) return false;


                Properties.Settings.Default.token = res.access_token;
                Properties.Settings.Default.refresh_token = res.refresh_token;
                Properties.Settings.Default.Save();
                return true;
                
            }
            catch (FlurlHttpException ex)
            {
                return false;
            }
        }


        public async Task<bool> RequestRegisterAsync(String email, String password, String confirmPassword)
        {
            try
            {

                var res = await RES_URL
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
                return false;
            }
        }

        public async Task<List<OrdersResponseModel>> GetPlacedOrders() {
            try
            {
              

                var result = await providePolicy().ExecuteAsync(() =>
                     RES_URL
                    .AppendPathSegment("/api/orders/placedOrders")
                    .WithOAuthBearerToken(Properties.Settings.Default.token)
                    .GetAsync()
                    .ReceiveJson<List<OrdersResponseModel>>());

                return result;

             }

            catch (FlurlHttpException ex)
            {
                return null;
            }
        }


        public async Task<bool> FinalizeOrder(int id, string status) {
            try
            {

              
                var result = await providePolicy().ExecuteAsync(() =>
                     RES_URL
                    .AppendPathSegment("/api/orders/finalizeOrder")
                    .WithOAuthBearerToken(Properties.Settings.Default.token)
                    .PutJsonAsync(new
                     {
                         Id = id,
                         Status = status,
                     }));


                return result.IsSuccessStatusCode;

            }
            catch (FlurlHttpException ex)
            {
                return false;
            }
        }

        public async Task<List<UsersResponseModel>> GetUsers()
        {
            try
            {

             

                var result = await providePolicy().ExecuteAsync(() =>
                      RES_URL
                    .AppendPathSegment("/api/accounts/users")
                    .WithOAuthBearerToken(Properties.Settings.Default.token)
                    .GetAsync()
                    .ReceiveJson<List<UsersResponseModel>>());


                return result;
            }

            catch (FlurlHttpException ex)
            {
                return null;
            }
        }



        public async Task<List<RoleResponseModel>> GetRoles()
        {
            try
            {
                var result = await providePolicy().ExecuteAsync(() =>  
                     RES_URL
                    .AppendPathSegment("/api/roles")
                    .WithOAuthBearerToken(Properties.Settings.Default.token)
                    .GetAsync()
                    .ReceiveJson<List<RoleResponseModel>>() );
                return result;

            }

            catch (FlurlHttpException ex)
            {
                return null;
            }
        }


        public async Task<bool> ManageUsersInRole(RoleBindingModel model)
        {
            try
            {
                var result = await providePolicy().ExecuteAsync(() =>
                   RES_URL
                  .AppendPathSegment("/api/roles/ManageUsersInRole")
                  .WithOAuthBearerToken(Properties.Settings.Default.token)
                   .PostJsonAsync(model));

                return result.IsSuccessStatusCode;

            }
            catch (FlurlHttpException ex)
            {
                return false;
            }
        }


    }
}
