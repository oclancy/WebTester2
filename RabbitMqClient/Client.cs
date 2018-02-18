using Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMqClient
{
    public class Client
    {
        public string Exchange { get; private set; }
        public string Queue { get; private set; }
        public string[] Topics { get; private set; }

        private IModel m_channel;
        private IConnection m_connection;
        private EventingBasicConsumer m_consumer;

        public event EventHandler<IMessage> MessageRecieved;

        public Client(string host, string exchange, string queneName)
        {
            Exchange = exchange;
            Queue = queneName;

            var factory = new ConnectionFactory() { HostName = host };
            m_connection = factory.CreateConnection();
            
            m_channel = m_connection.CreateModel();
            m_channel.ExchangeDeclare(Exchange, "direct", true, false, new Dictionary<string, object>());
            var queue = m_channel.QueueDeclare(Queue, false, false, false, new Dictionary<string, object>());
            m_channel.QueueBind(queue.QueueName, Exchange, string.Empty, new Dictionary<string, object>());
            
        }

        public Client(string host, string exchange, string queneName, string[] topics)
            :this( host,  exchange,  queneName)
        {
            Topics = topics;
        }

        public async void Start(CancellationToken cancelToken)
        {
            m_consumer = new EventingBasicConsumer(m_channel);
            await Task.Run( () => m_consumer.Received += Consumer_Received, cancelToken );
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            MessageRecieved?.Invoke(this, e.Body.Deserialize());
        }

        public void Send<T>( T message ) where T : IMessage
        {
            var props = m_channel.CreateBasicProperties();

            m_channel.BasicPublish(Exchange, Queue, props, message.Serialise());
        }
    }
}
