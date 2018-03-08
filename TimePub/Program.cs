
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
        static CancellationTokenSource m_cancellationTokenSrc = new CancellationTokenSource();
        static MessageDespatcher m_despatcher = new MessageDespatcher();

        public static MessageDespatcher Despatcher { get => m_despatcher; set => m_despatcher = value; }

        static void Main(string[] args)
        {
            
            var client = new RabbitMqClient.Client(args[0], args[1], args[2]);

            var keyPressedSource = Observable.FromEvent<ConsoleCancelEventHandler, ConsoleCancelEventArgs>(h =>
                                    {
                                        ConsoleCancelEventHandler handler = ( s, e ) => { h(e); };
                                        return handler;
                                    },
                                    ev => Console.CancelKeyPress += ev,
                                    ev => Console.CancelKeyPress -= ev);

            var observable = Observable.FromEvent<EventHandler<IMessage>, IMessage>(h => {
                                                        EventHandler<IMessage> handler = ( s, e ) => { h(e); };
                                                        return handler;
                                                        },
                                                        handler => client.MessageRecieved += handler,
                                                        handler => client.MessageRecieved -= handler);

            observable
                .TakeUntil(keyPressedSource)
                .Subscribe(Despatcher, m_cancellationTokenSrc.Token);


            var projhandler = new Handler(m_cancellationTokenSrc);

            Despatcher.RegisterHandler<StopMessage>(projhandler);
            Despatcher.RegisterHandler<TimeMessage>(projhandler);

            var source = Observable.Interval(new TimeSpan(0, 0, 1));
            source.TakeUntil(keyPressedSource)
                  .Subscribe(( time ) => client.Send(new TimeMessage() { Time = time }), m_cancellationTokenSrc.Token);

            client.Start();
            observable.Wait();
        }
    }
}
