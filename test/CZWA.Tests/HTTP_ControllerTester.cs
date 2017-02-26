using CZWA.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace CZWA.Tests
{
    public class HTTP_ControllerTester
    {
        protected TestServer _server;
        protected HttpClient _client;
        protected FormUrlEncodedContent _postLoginContent;

        public HTTP_ControllerTester()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"../../src/CZWA.Web/");

            _server = new TestServer(new WebHostBuilder().UseContentRoot(path).UseStartup<Startup>());
            Assert.IsNotNull(_server);

            _client = _server.CreateClient();
            Assert.IsNotNull(_client);

            _client.BaseAddress = new Uri("http://localhost:63497/");

            _postLoginContent = _getLoginContent();
            Assert.IsNotNull(_postLoginContent);
        }

        public async Task Get(string path, HttpStatusCode expectedHttpStatusCode)
        {
            await Task.Run(async () =>
            {
                var response = await _client.GetAsync(path);
                Assert.AreEqual(expectedHttpStatusCode, response.StatusCode);
            });
        }

        public async Task<HttpResponseMessage> Login()
        {
            return await Task.Run(async () =>
            {
                var response = await _client.PostAsync("/Account/Login", _postLoginContent);
                Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);
                response = await _client.SendAsync(_getRequest("Admin/", response));
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                return response;
            });
        }

        public async Task<HttpResponseMessage> Logout()
        {
            return await Task.Run(async () =>
            {
                var response = await _client.GetAsync("/Account/Logout");
                Assert.AreEqual(HttpStatusCode.Redirect, response.StatusCode);

                return response;
            });
        }

        public async Task<HttpResponseMessage> CheckAuthContent(HttpResponseMessage response)
        {
            return await Task.Run(async () =>
            {
                var body = await response.Content.ReadAsStringAsync();
                Assert.IsTrue(body.Trim().Length > 10);

                return response;
            });
        }

        public async Task<HttpResponseMessage> Send(HttpResponseMessage response, string path, HttpStatusCode expectedHttpStatusCode)
        {
            return await Task.Run(async () =>
            {
                response = await _client.SendAsync(_getRequest(path, response));
                Assert.AreEqual(expectedHttpStatusCode, response.StatusCode);

                return response;
            });
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
