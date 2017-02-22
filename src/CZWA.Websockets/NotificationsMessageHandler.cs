using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace CZWA.WebSockets
{
    public class NotificationsMessageHandler : WebSocketHandler
    {
        public NotificationsMessageHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public async void TestMethode(WebSocket socket,string name)
        {
            await InvokeClientMethodAsync(socket,"receiveMessage", "JoJo");
        }
    }
}
