using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using CZWA.Common;
using Microsoft.AspNetCore.Http;

namespace CZWA.Common
{
    public interface IWebSocketHandler<IMessage>
    {
        Task OnConnected(WebSocket socket);
        Task OnDisconnected(WebSocket socket);
        Task SendMessageAsync(WebSocket socket, IMessage message);
        Task SendMessageToAllAsync(IMessage Message);
        Task InvokeClientMethodAsync(WebSocket socket, string methodName, params object[] arguments);
        Task InvokeClientMethodAsync(string socketId, string methodName, params object[] arguments);
        Task InvokeClientMethodToAllAsync(string methodName, params object[] arguments);
        Task ReceiveAsync(WebSocket socket, HttpContext context, WebSocketReceiveResult result, byte[] buffer);
    }

    public interface IMessage<T>
    {
        T MessageType { get; set; }
        string Data { get; set; }
    }
}
