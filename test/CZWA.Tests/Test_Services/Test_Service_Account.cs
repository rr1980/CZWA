﻿using System.Runtime.CompilerServices;
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
using System.Reflection;
using CZWA.ViewModels;

[assembly: InternalsVisibleTo("Library.Tests")]
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


        [TestMethod]
        [TestCategory("Smoke")]
        public async Task GetUserById()
        {
            await Task.Run(async () =>
            {
                var user = await _accountService.GetUserById(1);

                Assert.IsNotNull(user);
                Assert.AreEqual("Riesner", user.Name);
                Assert.AreEqual("rr1980", user.ShowName);
                Assert.AreEqual(1, user.UserId);
                Assert.AreEqual("rr1980", user.Username);
                Assert.AreEqual("Rene", user.Vorname);
            });
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task _hasRole()
        {
            await Task.Run(async () =>
            {
                var user = await _accountService.GetUserById(2);
                Assert.IsNotNull(user);

                var hasRole = (bool)typeof(AccountService).GetMethod("_hasRole", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_accountService, new object[] { user, UserRoleType.Default });
                Assert.IsTrue(hasRole);
                hasRole = (bool)typeof(AccountService).GetMethod("_hasRole", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_accountService, new object[] { user, UserRoleType.Admin });
                Assert.IsFalse(hasRole);
            });
        }

        [TestMethod]
        [TestCategory("Smoke")]
        public async Task _getUserByNameAndPw()
        {
            await Task.Run(async () =>
            {
                var user = await (Task<UserViewModel>)typeof(AccountService).GetMethod("_getUserByNameAndPw", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_accountService, new object[] { "rr1980", "123" });
                Assert.IsNull(user);
                user = await (Task<UserViewModel>)typeof(AccountService).GetMethod("_getUserByNameAndPw", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(_accountService, new object[] { "rr1980", "12003" });
                Assert.IsNotNull(user);
                Assert.AreEqual("Riesner", user.Name);
                Assert.AreEqual("rr1980", user.ShowName);
                Assert.AreEqual(1, user.UserId);
                Assert.AreEqual("rr1980", user.Username);
                Assert.AreEqual("Rene", user.Vorname);

            });
        }
    }
}






