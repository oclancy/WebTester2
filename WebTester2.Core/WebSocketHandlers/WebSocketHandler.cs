using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messages;
using Microsoft.AspNetCore.Http;
using RabbitMqClient;
using WebTester2.WebSockets.WebSocketManager;
using static WebTester2.WebSockets.WebSocketHandlers.WebSocketConnection;

namespace WebTester2.WebSockets.WebSocketHandlers
{
    public class WebSocketHandler : AbstractWebSocketHandler
    {
        private readonly Client m_publisher;

        public WebSocketHandler(Client publisher ) : base()
        {
            this.m_publisher = publisher;
        }

        protected override int BufferSize { get => 1024 * 4; }

        public override async Task<AbstractWebSocketConnection> OnConnected(HttpContext context)
        {
            var name = context.Request.Query["Id"];
            AbstractWebSocketConnection connection = null;

            if (!string.IsNullOrEmpty(name))
            {
                connection = Connections.FirstOrDefault(m => ((WebSocketConnection)m).Id == name);
            }

            if (connection == null)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                connection = new WebSocketConnection(this, Publish)
                {
                    Id = Guid.NewGuid().ToString(),
                    WebSocket = webSocket
                };

                Connections.Add(connection);

                await connection.Init();
            }

            return connection;
        }

        private Task Publish( ReceiveMessage message )
        {
            m_publisher.Send(Translate(message));

            return Task.FromResult(message);
        }

        private IMessage Translate( ReceiveMessage message )
        {
            switch (message.Op)
            {
                case "request-price":
                    return PriceRequestMessage.Create(message.Data);
                default:
                    return null;
            }
        }
    }
}
