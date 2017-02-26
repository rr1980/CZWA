//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using CZWA.Common;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.TestHost;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace CZWA.Tests.Test
//{
//    [TestClass, Area("Rene")]
//    public class TestClass
//    {
//        private readonly ConcurrentQueue<HttpContext> _httpContexts = new ConcurrentQueue<HttpContext>();
//        private TestServer _testServer;

//        [TestInitialize]
//        public void CreateTestServer()
//        {
//            // This middleware stores all HTTP contexts created by the test server to be inspected by our tests.
//            Action<IApplicationBuilder> captureHttpContext = builder => builder.Use(async (httpContext, requestHandler) =>
//            {
//                await requestHandler.Invoke();
//                _httpContexts.Enqueue(httpContext);
//            });

//            var webHostBuilder = WebHostBuilderFactory.Create(new[]
//            {
//                captureHttpContext
//            });

//            _testServer = new TestServer(webHostBuilder);
//        }


//        public void DisposeTestServer()
//        {
//            _testServer.Dispose();
//        }

//        [TestMethod]
//        public void AnonymousUsersAreRedirectedToSignIn()
//        {
//            // Arrange
//            var browser = new TestServerBrowser(_testServer);

//            // Act
//            var frontPageResponse = browser.Get("/");

//            // Assert
//            Assert.AreEqual(frontPageResponse.StatusCode, HttpStatusCode.Found);
//            //Assert.AreEqual(frontPageResponse.Headers.Location.ToString(), Does.EndWith("/Authentication/SignIn?ReturnUrl=%2F"));
//        }

//        [TestMethod]
//        public void SuccessfulSignInRedirectsToFrontPage()
//        {
//            // Arrange
//            var browser = new TestServerBrowser(_testServer);
//            var credentials = new Dictionary<string, string>
//                {
//                    {"Username", "rr1980"},
//                    {"Password", "12003"},
//                    {"ReturnUrl", "/"}
//                };

//            // Act
//            var signInResponse = browser.Post("/Account/Login", credentials);

//            // Assert
//            Assert.AreEqual(signInResponse.StatusCode, HttpStatusCode.Found);
//            //Assert.That(signInResponse.Headers.Location.ToString(), Is.EqualTo("/"));
//        }

//        [TestMethod]
//        public void UserNameIsStoredInClaim()
//        {
//            // Arrange
//            var browser = new TestServerBrowser(_testServer);
//            var expectedName = "rr1980";
//            var credentials = new Dictionary<string, string>
//                {
//                    {"username", expectedName},
//                    {"password", "12003"},
//                    {"ReturnUrl", "/"}
//                };

//            var signInResponse = browser.Post("/Account/Login", credentials);

//            // Act
//            browser.FollowRedirect(signInResponse);

//            var name = _httpContexts.Last().User.FindFirstValue(ClaimTypes.Name);

//            // Assert
//            Assert.AreEqual(name, expectedName);
//        }

//        [TestMethod]
//        public void RolesIsStoredInClaim()
//        {
//            // Arrange
//            var browser = new TestServerBrowser(_testServer);
//            var expectedName = "rr1980";
//            var credentials = new Dictionary<string, string>
//                {
//                    {"username", expectedName},
//                    {"password", "12003"},
//                    {"ReturnUrl", "/"}
//                };

//            var signInResponse = browser.Post("/Account/Login", credentials);

//            // Act
//            browser.FollowRedirect(signInResponse);

//            var user = _httpContexts.Last().User;
//            Assert.IsNotNull(user);

//            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role);
//            Assert.IsTrue(roles.Count()==2);

//            var role_adnin = roles.FirstOrDefault(r => r.Value == UserRoleType.Admin.ToString());
//            Assert.IsNotNull(role_adnin);

//            var role_default = roles.FirstOrDefault(r => r.Value == UserRoleType.Default.ToString());
//            Assert.IsNotNull(role_default);

//        }

//        [TestMethod]
//        public void UserNameIsNOTStoredInClaim()
//        {
//            // Arrange
//            var browser = new TestServerBrowser(_testServer);
//            var credentials = new Dictionary<string, string>
//                {
//                    {"username", "rr19801"},
//                    {"password", "12003"},
//                    {"ReturnUrl", "/"}
//                };

//            var signInResponse = browser.Post("/Account/Login", credentials);

//            // Act
//            browser.FollowRedirect(signInResponse);
//            var name = _httpContexts.Last().User.FindFirstValue(ClaimTypes.Name);

//            // Assert
//            Assert.IsNull(name);
//        }

//        [TestMethod]
//        public void PasswordWrong()
//        {
//            // Arrange
//            var browser = new TestServerBrowser(_testServer);
//            var credentials = new Dictionary<string, string>
//                {
//                    {"username", "rr1980"},
//                    {"password", "12"},
//                    {"ReturnUrl", "/"}
//                };

//            var signInResponse = browser.Post("/Account/Login", credentials);

//            // Act
//            browser.FollowRedirect(signInResponse);
//            var name = _httpContexts.Last().User.FindFirstValue(ClaimTypes.Name);

//            // Assert
//            Assert.IsNull(name);
//        }
//    }
//}