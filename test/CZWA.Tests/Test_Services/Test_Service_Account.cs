using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.Services;
using Microsoft.AspNetCore.Mvc;
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
        public void AllUsers()
        {
            var allUsers = _accountService.AllUsers;
            Assert.IsNotNull(allUsers);
            Assert.IsTrue(allUsers.Count() >= 2);
        }
    }
}
