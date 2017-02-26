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

namespace CZWA.Tests.AccountController
{
    [TestClass, Area("Rene")]
    public class Test_AdminController
    {
        //private HTTP_ControllerTester _httpCTester;



        //[TestInitialize]
        //[TestCategory("Smoke")]
        //public void TestInitialize()
        //{
        //    _httpCTester = new HTTP_ControllerTester();
        //}

        //#region HTTP
        //[TestMethod]
        //[TestCategory("Smoke")]
        //public async Task HTTP_GET_Index()
        //{
        //    var r = await _httpCTester.Login();
        //    r = await _httpCTester.CheckAuthContent(r);
        //    r = await _httpCTester.Logout();
        //    r = await _httpCTester.Send(r, "Admin/", HttpStatusCode.Redirect);
        //}

    }
}
