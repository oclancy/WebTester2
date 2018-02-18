
using Messages;
using NLog;
using System;
using System.Reactive.Linq;
using System.Threading;

namespace TimePub
{
    class Program
    {
        static Logger Logger = LogManager.GetCurrentClassLogger();
        static CancellationTokenSource m_cancellationToken = new CancellationTokenSource();
        static MessageDespatcher m_despatcher = new MessageDespatcher();

        public static MessageDespatcher Despatcher { get => m_despatcher; set => m_despatcher = value; }

        static void Main(string[] args)
        {
            
            var client = new RabbitMqClient.Client(args[1], args[2], args[3]);

            var observable = Observable.FromEvent<EventHandler<IMessage>, IMessage>(handler => client.MessageRecieved += handler,
                                                                                    handler => client.MessageRecieved -= handler);


            observable.Subscribe(Despatcher, m_cancellationToken.Token);

            var source = Observable.Interval(new TimeSpan(0, 0, 1));
            source.Subscribe((time) => client.Send(new TimeMessage() { Time = time }), m_cancellationToken.Token);

            var stopHandler = new Handler(m_cancellationToken);

            Despatcher.RegisterHandler<StopMessage>(stopHandler);

            observable.Wait();
        }
    }
}
