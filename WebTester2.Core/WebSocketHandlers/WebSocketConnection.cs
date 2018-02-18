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
        public WebSocketConnection( WebSocketHandler handler) : base(handler)
        {
        }

        public string Id { get; set; }

        public override async Task ReceiveAsync(string message)
        {
            var receiveMessage = JsonConvert.DeserializeObject<ReceiveMessage>(message);

            var receiver = Handler.Connections.FirstOrDefault(m => ((WebSocketConnection)m).Id == receiveMessage.Receiver);

            if (receiver != null)
            {
                var sendMessage = JsonConvert.SerializeObject(new SendMessage
                {
                    Sender = Id,
                    Message = receiveMessage.Message
                });

                await receiver.SendMessageAsync(sendMessage);
            }
            else
            {
                var sendMessage = JsonConvert.SerializeObject(new SendMessage
                {
                    Sender = Id,
                    Message = "Can not seed to " + receiveMessage.Receiver
                });

                await SendMessageAsync(sendMessage);
            }
        }

        private class ReceiveMessage
        {
            public string Receiver { get; set; }

            public string Message { get; set; }
        }

        private class SendMessage
        {
            public string Sender { get; set; }

            public string Message { get; set; }
        }
    }
}