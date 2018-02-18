using System.Threading;
using System.Threading.Tasks;
using Messages;
using Messages.Handlers;
using NLog;

namespace TimePub
{
    public class Handler : IHandleMessage<StopMessage>
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private CancellationTokenSource m_cancelTokenSource;

        public Handler(CancellationTokenSource token)
        {
            m_cancelTokenSource = token;
        }

        public Task Handle(StopMessage message)
        {
            Logger.Info(message.Reason);
            m_cancelTokenSource.Cancel();
            return Task.FromResult(true);
        }
    }
}

