using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace coffee_pc.Service
{
    class MainService
    {
        const String BASE_URL = "http://localhost:5819";


        public async Task<String> RequestLoginAsync(String email, String password)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(email);
                System.Diagnostics.Debug.WriteLine(password);

                String res = await BASE_URL
               .AppendPathSegment("/oauth2/token")
               .WithHeaders(new { Accept = "application/json", Content_Type = "application/x-www-form-urlencoded" }).PostUrlEncodedAsync(new
               {
                   username = email,
                   password = password,
                   grant_type = "password",
                   client_id = "24e5a184d2b1488c8dc97587625260fb"
               }).ReceiveString();

                return res;
            }
            catch (FlurlHttpException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return "";
            }
        }

    }
}
