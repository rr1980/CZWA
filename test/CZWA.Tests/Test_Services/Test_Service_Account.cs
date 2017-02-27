using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CZWA.Common;
using CZWA.DB_Migration;
using CZWA.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CZWA.Tests.Test_Services
{
    [TestClass, Area("Rene")]
    public class Test_Service_Account : Test_Base
    {
        private AccountService _accountService;

        [TestInitialize]
        [TestCategory("Smoke")]
        public new void TestInitialize()
        {
            base.TestInitialize();

            _accountService = (AccountService)_testServer.Host.Services.GetService(typeof(AccountService));
            Assert.IsNotNull(_accountService);
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GetAllUsers()
        {
            var allUsers = await _accountService.GetAllUsers();
            Assert.IsNotNull(allUsers);
            Assert.IsTrue(allUsers.Count() >= 2);
        }
    }
}






