using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using WebTester2.WebSockets.WebSocketManager;

namespace WebTester2.WebSockets.WebSocketHandlers
{
    public class WebSocketConnection : AbstractWebSocketConnection
    {
        private readonly Func<ReceiveMessage,Task> m_onMessageCallBack;

        public WebSocketConnection( WebSocketHandler handler, 
                                    Func<ReceiveMessage,Task> onMessageCallBack ) 
            : base(handler)
        {
            this.m_onMessageCallBack = onMessageCallBack;
        }

        public string Id { get; set; }

        public override async Task Init()
        {
            await SendMessageAsync(Id.ToString());
        }

        public override async Task ReceiveAsync(string message)
        {
            var receiveMessage = JsonConvert.DeserializeObject<ReceiveMessage>(message);

            var receiver = Handler.Connections
                                  .FirstOrDefault( m => ((WebSocketConnection)m).Id == receiveMessage.Id.ToString());

            if (receiver != null)
            {
               await m_onMessageCallBack(receiveMessage);
            }

        }

        public class ReceiveMessage
        {
            public Guid Id{ get; set; }

            public string Data { get; set; }

            public string Op { get; set; }
        }

    }
}