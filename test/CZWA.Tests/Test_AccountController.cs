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
        string path = @"C:\Users\rr1980\Documents\Visual Studio 2015\Projects\CZWA\src\CZWA.Web";
        //string path = @"D:\Projects\CZWA\src\CZWA.Web";

        private FormUrlEncodedContent _postLoginContent;
        string content_path = @"C:\Users\rr1980\Documents\Visual Studio 2015\Projects\CZWA\test\CZWA.Tests\";
        //string content_path = @"D:\Projects\CZWA\test\CZWA.Tests\";

        [TestInitialize]
        public void TestInitialize()
        {
            _server = new TestServer(new WebHostBuilder().UseContentRoot(path).UseStartup<Startup>());
            _client = _server.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:63497/");

            _postLoginContent = _getLoginContent();
            _homeContent = File.ReadAllText(content_path + "HomeContent.html.test");
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
        public async Task Post_Account_Login_Logout()
        {
            var response = await _client.PostAsync("/Account/Login", _postLoginContent);
            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);

            response = await _client.SendAsync(_getRequest("Home/", response));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(_homeContent.Trim(), body.Trim());

            response = await _client.GetAsync("/Account/Logout");
            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);

            response = await _client.SendAsync(_getRequest("Home/", response));
            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);
        }

        private FormUrlEncodedContent _getLoginContent()
        {
            var nameValueCollection = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Username", "rr1980"),
                new KeyValuePair<string, string>("Password", "12003"),
                new KeyValuePair<string, string>("ReturnUrl", "/"),
            };
            return new FormUrlEncodedContent(nameValueCollection);
        }


        private HttpRequestMessage _getRequest(string path, HttpResponseMessage response)
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
