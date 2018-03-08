using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebTester2.WebSockets.WebSocketManager;

namespace WebTester2.WebSockets.WebSocketHandlers
{
    public class WebSocketHandler : AbstractWebSocketHandler
    {
        protected override int BufferSize { get => 1024 * 4; }

        public override async Task<AbstractWebSocketConnection> OnConnected(HttpContext context)
        {
            var name = context.Request.Query["Id"];
            if (!string.IsNullOrEmpty(name))
            {
                var connection = Connections.FirstOrDefault(m => ((WebSocketConnection)m).Id == name);

                if (connection == null)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    connection = new WebSocketConnection(this)
                    {
                        Id = Guid.NewGuid().ToString(),
                        WebSocket = webSocket
                    };

                    Connections.Add(connection);
                }

                return connection;
            }

            return null;
        }
    }
}
