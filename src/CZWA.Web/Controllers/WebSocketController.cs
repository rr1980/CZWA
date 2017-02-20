﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CZWA.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace CZWA.Web.Controllers
{
    public class WebSocketController : Controller
    {
        private NotificationsMessageHandler _notificationsMessageHandler { get; set; }

        public WebSocketController(NotificationsMessageHandler notificationsMessageHandler)
        {
            _notificationsMessageHandler = notificationsMessageHandler;
        }

        [HttpGet]
        public async Task SendMessage([FromQueryAttribute]string message)
        {
            await _notificationsMessageHandler.InvokeClientMethodToAllAsync("receiveMessage", message);
        }
    }
}
