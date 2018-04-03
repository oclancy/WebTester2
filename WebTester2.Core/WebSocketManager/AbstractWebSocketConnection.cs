using Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebTester2.WebSockets.WebSocketManager
{
    public abstract class AbstractWebSocketConnection
    {
        public AbstractWebSocketHandler Handler { get; }

        public System.Net.WebSockets.WebSocket WebSocket { get; set; }

        public AbstractWebSocketConnection( AbstractWebSocketHandler handler)
        {
            Handler = handler;
        }

        public virtual async Task SendMessageAsync( IMessage message )
        {
            await SendMessageAsync(JsonConvert.SerializeObject(message));
        }

        public virtual async Task SendMessageAsync(string message)
        {
            if (WebSocket.State != WebSocketState.Open) return;
            var arr = Encoding.UTF8.GetBytes(message);

            var buffer = new ArraySegment<byte>(
                    array: arr,
                    offset: 0,
                    count: arr.Length);

            await WebSocket.SendAsync(
                buffer: buffer,
                messageType: WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: CancellationToken.None
                );
        }

        public abstract Task Init( );
       
        public abstract Task ReceiveAsync(string message);
    }
}
