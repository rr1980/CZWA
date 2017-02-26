using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CZWA.Tests
{
    public abstract class Test_Base
    {
        protected readonly ConcurrentQueue<HttpContext> _httpContexts = new ConcurrentQueue<HttpContext>();
        protected TestServer _testServer;

        [TestInitialize]
        [TestCategory("Smoke")]
        public void TestInitialize()
        {
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
    }
}
