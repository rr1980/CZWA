using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CZWA.Tests
{
    public class TestServerBrowser
    {
        private readonly TestServer _testServer;
        // Modify to match your XSRF token requirements.
        private const string XsrfCookieName = "XSRF-TOKEN";
        private const string XsrfHeaderName = "X-XSRF-TOKEN";

        public TestServerBrowser(TestServer testServer)
        {
            _testServer = testServer;
            Cookies = new CookieContainer();
        }

        public CookieContainer Cookies { get; }

        public async Task<HttpResponseMessage> Get(string relativeUrl)
        {
            return await Task.Run(async () =>
            {


                await Task.Delay(5000);
                var frontPageResponse = await Get(new Uri(relativeUrl, UriKind.Relative));
                //var frontPageResponse = await browser.Get("/");


                // Assert
                Assert.AreEqual(frontPageResponse.StatusCode, HttpStatusCode.Found);
                Assert.IsTrue(frontPageResponse.Headers.Location.ToString().Contains("/Account/Login?ReturnUrl=%2F"));

                return frontPageResponse;
            });
        }

        public async Task<HttpResponseMessage> Get(Uri relativeUrl)
        {
            var absoluteUrl = new Uri(_testServer.BaseAddress, relativeUrl);
            var requestBuilder = _testServer.CreateRequest(absoluteUrl.ToString());
            AddCookies(requestBuilder, absoluteUrl);
            var response = await requestBuilder.GetAsync();
            UpdateCookies(response, absoluteUrl);
            return response;
        }

        private void AddCookies(RequestBuilder requestBuilder, Uri absoluteUrl)
        {
            var cookieHeader = Cookies.GetCookieHeader(absoluteUrl);
            if (!string.IsNullOrWhiteSpace(cookieHeader))
            {
                requestBuilder.AddHeader(HeaderNames.Cookie, cookieHeader);
            }
        }

        private void UpdateCookies(HttpResponseMessage response, Uri absoluteUrl)
        {
            if (response.Headers.Contains(HeaderNames.SetCookie))
            {
                var cookies = response.Headers.GetValues(HeaderNames.SetCookie);
                foreach (var cookie in cookies)
                {
                    Cookies.SetCookies(absoluteUrl, cookie);
                }
            }
        }

        public async Task<HttpResponseMessage> Post(string relativeUrl, IDictionary<string, string> formValues)
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(5000);
                return await Post(new Uri(relativeUrl, UriKind.Relative), formValues);
            });
        }

        public async Task<HttpResponseMessage> Post(Uri relativeUrl, IDictionary<string, string> formValues)
        {
            var absoluteUrl = new Uri(_testServer.BaseAddress, relativeUrl);
            var requestBuilder = _testServer.CreateRequest(absoluteUrl.ToString());
            AddCookies(requestBuilder, absoluteUrl);
            SetXsrfHeader(requestBuilder, absoluteUrl);
            var content = new FormUrlEncodedContent(formValues);
            var response = await requestBuilder.And(message =>
            {
                message.Content = content;
            }).PostAsync();
            UpdateCookies(response, absoluteUrl);
            return response;
        }

        // Modify to match your XSRF token requirements, e.g. "SetXsrfFormField".
        private void SetXsrfHeader(RequestBuilder requestBuilder, Uri absoluteUrl)
        {
            var cookies = Cookies.GetCookies(absoluteUrl);
            var cookie = cookies[XsrfCookieName];
            if (cookie != null)
            {
                requestBuilder.AddHeader(XsrfHeaderName, cookie.Value);
            }
        }

        public async Task<HttpResponseMessage> FollowRedirect(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.Moved && response.StatusCode != HttpStatusCode.Found)
            {
                return response;
            }
            var redirectUrl = new Uri(response.Headers.Location.ToString(), UriKind.RelativeOrAbsolute);
            if (redirectUrl.IsAbsoluteUri)
            {
                redirectUrl = new Uri(redirectUrl.PathAndQuery, UriKind.Relative);
            }
            return await Get(redirectUrl);
        }
    }
}
