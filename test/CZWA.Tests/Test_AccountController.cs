using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CZWA.DB;
using CZWA.ViewModels;
using CZWA.Web;
using CZWA.Web.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CZWA.Tests
{
    [TestClass]
    public class Test_AccountController
    {
        private TestServer _server;
        private HttpClient _client;
        private string _homeContent;

        [TestInitialize]
        public void TestInitialize()
        {
            _server = new TestServer(new WebHostBuilder().UseContentRoot(@"C:\Users\rr1980\Documents\Visual Studio 2015\Projects\CZWA\src\CZWA.Web").UseStartup<Startup>());
            _client = _server.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:63497/");

            _homeContent = File.ReadAllText(@"C:\Users\rr1980\Documents\Visual Studio 2015\Projects\CZWA\test\CZWA.Tests\HomeContent.html.test");
        }

        [TestMethod]
        [TestCategory("Smoke")]
        [TestCategory("regression")]
        public void InitAspNet()
        {
            Assert.IsNotNull(_server);
            Assert.IsNotNull(_client);
        }

        [Description("test 123456"), TestCategory("Edit Tests"), TestCategory("Non-Smoke"), TestMethod]
        public async Task Get_Index()
        {
            var response = await _client.GetAsync("/");
            Assert.AreEqual(HttpStatusCode.Found, response.StatusCode);
        }

        [Description("test 123456"), TestCategory("Edit Tests"), TestCategory("Non-Smoke"), TestMethod]
        public async Task Get_Account_Login()
        {
            var response = await _client.GetAsync("/Account/Login");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Description("test 123456"), TestCategory("Edit Tests"), TestCategory("Non-Smoke"), TestMethod]
        public async Task Post_Account_Login()
        {
            LoginViewModel vm = new LoginViewModel()
            {
                Username = "rr1980",
                Password = "12003",
                ReturnUrl = "/"
            };

            var nameValueCollection = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Username", "rr1980"),
                new KeyValuePair<string, string>("Password", "12003"),
                new KeyValuePair<string, string>("ReturnUrl", "/"),
            };
            var content = new FormUrlEncodedContent(nameValueCollection);

            var response = await _client.PostAsync("/Account/Login", content);
            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);

            response = await _client.SendAsync(GetRequest("Home/", response));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(_homeContent.Trim(), body.Trim());
        }

        private HttpRequestMessage GetRequest(string path, HttpResponseMessage response)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            IEnumerable<string> values;
            if (response.Headers.TryGetValues("Set-Cookie", out values))
            {
                var cookie = SetCookieHeaderValue.ParseList(values.ToList()).First();
                request.Headers.Add("Cookie", new CookieHeaderValue(cookie.Name, cookie.Value).ToString());
            }

            return request;
        }
    }
}
