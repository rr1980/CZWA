using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using CZWA.Common;
using CZWA.Services;
using CZWA.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CZWA.WebSockets
{
    public class AdminMessageHandler : WebSocketHandler
    {
        private readonly UserRoleType urt = UserRoleType.Admin;
        private readonly LoginService _loginService;

        public AdminMessageHandler(WebSocketConnectionManager webSocketConnectionManager, LoginService loginService) : base(webSocketConnectionManager)
        {
            _loginService = loginService;
        }

        public async void SaveUser(WebSocket socket, dynamic user)
        {
            if (!_loginService.HasRole(urt))
            {
                return;
            }
            var usr = JsonConvert.DeserializeObject<UserViewModel>(user.ToString());

            await InvokeClientMethodAsync(socket,"receiveMessage", "ADMIN");
        }
    }
}
