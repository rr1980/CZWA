using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using CZWA.Common;
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

namespace CZWA.Tests.Test_Controller
{
    [TestClass, Area("Rene")]
    public class Test_Controller_Account : Test_Base
    {
        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_GET_RedirectedTo_Login()
        {
            var browser = new TestServerBrowser(_testServer);

            var frontPageResponse = await browser.Get("/");

            Assert.AreEqual(frontPageResponse.StatusCode, HttpStatusCode.Found);
            Assert.IsTrue(frontPageResponse.Headers.Location.ToString().Contains("/Account/Login?ReturnUrl=%2F"));
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_POST_Login_RedirectedTo_Home()
        {
            var browser = new TestServerBrowser(_testServer);
            var credentials = new Dictionary<string, string>
                            {
                                {"Username", "rr1980"},
                                {"Password", "12003"},
                                {"ReturnUrl", "/"}
                            };

            var signInResponse = await browser.Post("/Account/Login", credentials);
            Assert.AreEqual(signInResponse.StatusCode, HttpStatusCode.Found);
            Assert.IsTrue(signInResponse.Headers.Location.ToString().Contains("/"));
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_POST_Login_Right_Username()
        {
            var browser = new TestServerBrowser(_testServer);
            var expectedName = "rr1980";
            var credentials = new Dictionary<string, string>
                        {
                            {"username", expectedName},
                            {"password", "12003"},
                            {"ReturnUrl", "/"}
                        };

            var signInResponse = await browser.Post("/Account/Login", credentials);

            await browser.FollowRedirect(signInResponse);

            var name = _httpContexts.Last().User.FindFirstValue(ClaimTypes.Name);
            Assert.AreEqual(name, expectedName);
        }

        [TestMethod]
        public async Task HTTP_POST_Login_Wrong_Username()
        {
            var browser = new TestServerBrowser(_testServer);
            var credentials = new Dictionary<string, string>
                        {
                            {"username", "rr19801"},
                            {"password", "12003"},
                            {"ReturnUrl", "/"}
                        };

            var signInResponse = await browser.Post("/Account/Login", credentials);

            await browser.FollowRedirect(signInResponse);

            var name = _httpContexts.Last().User.FindFirstValue(ClaimTypes.Name);
            Assert.IsNull(name);
        }

        [TestMethod]
        public async Task HTTP_POST_Login_Right_Role()
        {
            var browser = new TestServerBrowser(_testServer);
            var expectedName = "rr1980";
            var credentials = new Dictionary<string, string>
                        {
                            {"username", expectedName},
                            {"password", "12003"},
                            {"ReturnUrl", "/"}
                        };

            var signInResponse = await browser.Post("/Account/Login", credentials);

            await browser.FollowRedirect(signInResponse);

            var user = _httpContexts.Last().User;
            Assert.IsNotNull(user);

            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role);
            Assert.IsTrue(roles.Count() == 2);

            var role_adnin = roles.FirstOrDefault(r => r.Value == UserRoleType.Admin.ToString());
            Assert.IsNotNull(role_adnin);

            var role_default = roles.FirstOrDefault(r => r.Value == UserRoleType.Default.ToString());
            Assert.IsNotNull(role_default);
        }

        [TestMethod]
        public async Task HTTP_POST_Login_Wrong_Password()
        {
            var browser = new TestServerBrowser(_testServer);
            var credentials = new Dictionary<string, string>
                        {
                            {"username", "rr1980"},
                            {"password", "12"},
                            {"ReturnUrl", "/"}
                        };

            var signInResponse = await browser.Post("/Account/Login", credentials);

            await browser.FollowRedirect(signInResponse);

            var name = _httpContexts.Last().User.FindFirstValue(ClaimTypes.Name);
            Assert.IsNull(name);
        }
    }
}
