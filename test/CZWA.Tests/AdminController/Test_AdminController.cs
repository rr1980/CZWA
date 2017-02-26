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
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;
using CZWA.Common;

namespace CZWA.Tests.AccountController
{
    [TestClass, Area("Rene")]
    public class Test_AdminController : Test_BaeController
    {
        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_GET_Admin()
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
            
            var frontPageResponse = await browser.Get("/Admin");
            Assert.AreEqual(HttpStatusCode.OK, frontPageResponse.StatusCode);
            Assert.AreEqual(frontPageResponse.RequestMessage.RequestUri.AbsolutePath, "/Admin");

            var user = _httpContexts.Last().User;
            Assert.IsNotNull(user);

            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role);
            Assert.IsTrue(roles.Count() == 2);

            var role_adnin = roles.FirstOrDefault(r => r.Value == UserRoleType.Admin.ToString());
            Assert.IsNotNull(role_adnin);

            var role_default = roles.FirstOrDefault(r => r.Value == UserRoleType.Default.ToString());
            Assert.IsNotNull(role_default);
        }

    }
}
