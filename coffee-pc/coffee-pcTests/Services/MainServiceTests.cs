using Microsoft.VisualStudio.TestTools.UnitTesting;
using coffee_pc.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http.Testing;
using System.Net.Http;
using Flurl.Http;

namespace coffee_pc.Service.Tests
{
    [TestClass()]
    public class MainServiceTests
    {
        const String RES_URL = "http://localhost:5819";
        const String AUTH_URL = "http://localhost:5821";
        MainService ms = new MainService();

        [TestMethod()]
        public async Task RequestLoginAsyncTest()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith("OK", 200);

                await ms.RequestLoginAsync("valami@mail.com", "asdasd123");

                httpTest.ShouldHaveCalled($"{AUTH_URL}/oauth2/token")
                .WithVerb(HttpMethod.Post)
                .WithContentType("application/x-www-form-urlencoded")
                .Times(1);
            } 
        }

        [TestMethod()]
        public async Task RequestRegisterAsyncTest()
        {
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWith("OK", 200);

                await ms.RequestRegisterAsync("valami@mail.com", "asdasd123", "asdasd123");

                httpTest.ShouldHaveCalled($"{RES_URL}/api/accounts/create")
                .WithVerb(HttpMethod.Post)
                .WithContentType("application/json")
                .Times(1);
            }
        }
    }
}