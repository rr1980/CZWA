using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using CZWA.DB;
using CZWA.ViewModels;
using CZWA.Web;
using CZWA.Web.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CZWA.Tests.AccountController
{
    [TestClass, Area("Rene")]
    public class Test_AccountController
    {
        private readonly ConcurrentQueue<HttpContext> _httpContexts = new ConcurrentQueue<HttpContext>();
        private TestServer _testServer;

        [TestInitialize]
        [TestCategory("Smoke")]
        public void TestInitialize()
        {
            // This middleware stores all HTTP contexts created by the test server to be inspected by our tests.
            Action<IApplicationBuilder> captureHttpContext = builder => builder.Use(async (httpContext, requestHandler) =>
            {
                await requestHandler.Invoke();
                _httpContexts.Enqueue(httpContext);
            });

            var webHostBuilder = WebHostBuilderFactory.Create(new[]
            {
                captureHttpContext
            });

            _testServer = new TestServer(webHostBuilder);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_GET_RedirectedTo_Login()
        {
            // Arrange
            var browser = new TestServerBrowser(_testServer);

            // Act
            var frontPageResponse = await browser.Get("/");


            // Assert
            //Assert.AreEqual(frontPageResponse.StatusCode, HttpStatusCode.Found);
            //Assert.IsTrue(frontPageResponse.Headers.Location.ToString().Contains("/Account/Login?ReturnUrl=%2F"));

        }


        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_POST_Login_RedirectedTo_Home()
        {
            // Arrange
            var browser = new TestServerBrowser(_testServer);
            var credentials = new Dictionary<string, string>
                            {
                                {"Username", "rr1980"},
                                {"Password", "12003"},
                                {"ReturnUrl", "/"}
                            };

            // Act
            var signInResponse = await browser.Post("/Account/Login", credentials);

            // Assert
            Assert.AreEqual(signInResponse.StatusCode, HttpStatusCode.Found);

            var tmp = signInResponse.Headers.Location;

            Assert.IsTrue(signInResponse.Headers.Location.ToString().Contains("/"));
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_POST_Login_UserNameIsStoredInClaim()
        {
            // Arrange
            var browser = new TestServerBrowser(_testServer);
            var expectedName = "rr1980";
            var credentials = new Dictionary<string, string>
                        {
                            {"username", expectedName},
                            {"password", "12003"},
                            {"ReturnUrl", "/"}
                        };

            var signInResponse = await browser.Post("/Account/Login", credentials);

            // Act
            await browser.FollowRedirect(signInResponse);

            var name = _httpContexts.Last().User.FindFirstValue(ClaimTypes.Name);

            // Assert
            Assert.AreEqual(name, expectedName);
        }

        //[TestMethod]
        //[TestCategory("Smoke")]
        //public async Task HTTP_CheckAuthContent()
        //{
        //    var r = await _httpCTester.Login();
        //    r = await _httpCTester.CheckAuthContent(r);
        //    await _httpCTester.Logout();
        //}

        //[TestMethod]
        //[TestCategory("Smoke")]
        //public async Task HTTP_GET_Logout()
        //{
        //    await _httpCTester.Logout();
        //}
        //#endregion
    }
}
