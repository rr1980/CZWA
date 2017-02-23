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
        private readonly LoginService _loginService;

        public NotificationsMessageHandler(WebSocketConnectionManager webSocketConnectionManager, LoginService loginService) : base(webSocketConnectionManager)
        {
            _loginService = loginService;
        }

        public async void TestMethode(WebSocket socket,string name)
        {
            if (!_loginService.HasRole(urt))
            {
                return;
            }

            await InvokeClientMethodAsync(socket,"receiveMessage", "JoJo");
        }
    }
}
