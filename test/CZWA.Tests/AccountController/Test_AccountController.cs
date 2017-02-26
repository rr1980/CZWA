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

namespace CZWA.Tests.AccountController
{
    [TestClass, Area("Rene")]
    public class Test_AccountController
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
        public async Task HTTP_GET_Login()
        {
            await _httpCTester.Get("/", HttpStatusCode.Redirect);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_POST_Login()
        {
            await _httpCTester.Login();
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_CheckAuthContent()
        {
            var r = await _httpCTester.Login();
            r = await _httpCTester.CheckAuthContent(r);
            await _httpCTester.Logout();
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task HTTP_GET_Logout()
        {
            await _httpCTester.Logout();
        }
        #endregion
    }
}
