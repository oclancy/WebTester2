using Messages.Handlers;
using NLog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Messages
{
    public class MessageDespatcher : IObserver<IMessage>
    {
        public delegate Task HandlerDelegate(IMessage message);

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private Dictionary<Type, HandlerDelegate> m_callbackDict = new Dictionary<Type, HandlerDelegate>();

        public void OnCompleted()
        {
            Logger.Info("Observer finished.");
        }

        public void OnError(Exception error)
        {
            Logger.Error(error);
        }

        public void OnNext(IMessage value)
        {
            Logger.Info($"Message recieved: {value}");
            var type = value.GetType();

            if (m_callbackDict.ContainsKey(type))
                m_callbackDict[type].Invoke(value);
        }

        public void RegisterHandler( Type messageType, object handler )
        {
            var method =handler.GetType()
                               .GetMethod("Handle", 
                                    BindingFlags.InvokeMethod | BindingFlags.Instance ,
                                    null, 
                                    new[] { messageType } ,
                                    null);

            m_callbackDict.Add(messageType, ( message ) => (Task)method.Invoke( handler, new[] { message }));
        }

        public void RegisterHandler<T>(IHandleMessage<T> handler) where T : IMessage
        {
            m_callbackDict.Add(typeof(T), (message) => handler.Handle( (T)message ) );
        }
    }
}
