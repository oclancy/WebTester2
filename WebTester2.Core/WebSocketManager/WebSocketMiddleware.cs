using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTester2.WebSockets.WebSocketManager
{
    /// <summary>
    /// This should the last middleware in the pipeline when use websocket
    /// </summary>
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private AbstractWebSocketHandler _webSocketHandler { get; set; }

        public WebSocketMiddleware(RequestDelegate next, AbstractWebSocketHandler webSocketHandler )
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var connection = await _webSocketHandler.OnConnected(context);
                if (connection != null)
                {
                    await _webSocketHandler.ListenConnection(connection);
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            }
        }
    }
}
