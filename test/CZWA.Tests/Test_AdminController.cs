using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CZWA.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CZWA.Tests
{
    [TestClass, Area("Rene")]
    public class Test_AdminController
    {
        private TestServer _server;
        private HttpClient _client;
        private FormUrlEncodedContent _postLoginContent;
        //private string _adminContent;

        string path = @"C:\Users\rr1980\Documents\Visual Studio 2015\Projects\CZWA\src\CZWA.Web";
        //string path = @"D:\Projects\CZWA\src\CZWA.Web";
        //string content_path = @"C:\Users\rr1980\Documents\Visual Studio 2015\Projects\CZWA\test\CZWA.Tests\";
        //string content_path = @"D:\Projects\CZWA\test\CZWA.Tests\";

        [TestInitialize]
        [TestCategory("Smoke")]
        public void TestInitialize()
        {
            _server = new TestServer(new WebHostBuilder().UseContentRoot(path).UseStartup<Startup>());
            _client = _server.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:63497/");

            _postLoginContent = _getLoginContent();
            //_adminContent = File.ReadAllText(content_path + "AdminContent.html.test");
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public void InitAspNet()
        {
            Assert.IsNotNull(_server);
            Assert.IsNotNull(_client);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task Get_Index()
        {
            var response = await _client.GetAsync("/Admin");
            Assert.AreEqual(HttpStatusCode.Found, response.StatusCode);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task Post_Admin_Login_Logout()
        {
            var response = await _client.PostAsync("/Account/Login", _postLoginContent);
            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);

            response = await _client.SendAsync(_getRequest("Admin/", response));
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            //Assert.AreEqual(_adminContent.Trim(), body.Trim());
            Assert.IsTrue(body.Trim().Length>10);

            // Todo Post und check Benutzer änderungen speichern


            response = await _client.GetAsync("/Account/Logout");
            Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);

            response = await _client.SendAsync(_getRequest("Admin/", response));
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
