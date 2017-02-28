using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading.Tasks;
using CZWA.Common;
using CZWA.Services;
using Microsoft.AspNetCore.Http;

namespace CZWA.WebSockets
{
    public class NotificationsMessageHandler : WebSocketHandler
    {
        private readonly UserRoleType urt = UserRoleType.Default;
        private readonly AccountService _accountService;

        public NotificationsMessageHandler(WebSocketConnectionManager webSocketConnectionManager, AccountService accountService) : base(webSocketConnectionManager)
        {
            _accountService = accountService;
        }

        public async void TestMethode(WebSocket socket, HttpContext httpContext, string name)
        {
            var id = Convert.ToInt32(httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            if (!await _accountService.HasRole(id, urt))
            {
                return;
            }

            await InvokeClientMethodAsync(socket, "receiveMessage", "JoJo");
        }
    }
}
