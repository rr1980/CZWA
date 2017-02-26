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

namespace CZWA.Tests.AdminController
{
    [TestClass, Area("Rene")]
    public class Test_AdminController
    {
        private HTTP_ControllerTester _httpCTester;


        [TestInitialize]
        [TestCategory("Smoke")]
        public void TestInitialize()
        {
            _httpCTester = new HTTP_ControllerTester();
        }

        #region HTTP
        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_InitAspNet()
        {
            await _httpCTester.InitAspNet();
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_GET()
        {
            await _httpCTester.Get("/Admin", HttpStatusCode.Redirect);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_POST_Admin_Login_Logout()
        {
            var r = await _httpCTester.Login();
            r = await _httpCTester.Logout();
            r = await _httpCTester.Send(r, "Admin/", HttpStatusCode.Redirect);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_CheckAuthContent()
        {
            var r = await _httpCTester.Login();
            r = await _httpCTester.CheckAuthContent(r);
            r = await _httpCTester.Logout();
        }
        #endregion
    }
}
