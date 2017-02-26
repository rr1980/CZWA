using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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

        public async void TestMethode(WebSocket socket, string name)
        {
            if (!await _accountService.HasRole(urt))
            {
                return;
            }

            await InvokeClientMethodAsync(socket, "receiveMessage", "JoJo");
        }
    }
}
