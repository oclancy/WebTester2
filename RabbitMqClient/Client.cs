using Messages;
using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;


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

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public Client(string host, string exchange, string queueName)
        {
            Exchange = exchange;
            Queue = queueName;

            var factory = new ConnectionFactory()
            {
                HostName = host,
                UserName = "webtester",
                Password = "webtester"
            };

            m_connection = factory.CreateConnection();
            
            m_channel = m_connection.CreateModel();
            m_channel.ExchangeDeclare(Exchange, "direct", true, false, new Dictionary<string, object>());
            var queue = m_channel.QueueDeclare(Queue, false, false, false, new Dictionary<string, object>());

            m_channel.ModelShutdown += ( s, e ) => m_connection.Close();
            
        }

        public Client(string host, string exchange, string queueName, string[] topics)
            :this( host,  exchange,  queueName)
        {
            Topics = topics;
        }

        public IPublisherMap PublisherMap { get;set;}

        public void Start()
        {
            foreach(var topic in Topics)
                m_channel.QueueBind(Queue, Exchange, topic, new Dictionary<string, object>());

            m_consumer = new EventingBasicConsumer(m_channel);
            m_consumer.Received += Consumer_Received;
            m_channel.BasicConsume(Queue, true, m_consumer);
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = e.Body.Deserialize();
            Logger.Info($"recieved {message}");
            MessageRecieved?.Invoke(this, message );
        }

        public void Send<T>( T message ) where T : IMessage
        {
            Logger.Info($"sending {message}");
            var props = m_channel.CreateBasicProperties();

            var topic = PublisherMap?.GetTopic(message) ?? Queue;

            m_channel.BasicPublish(Exchange, Queue, props, message.Serialise());
        }
    }
}
