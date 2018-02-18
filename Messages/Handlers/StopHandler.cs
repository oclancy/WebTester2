using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Handlers
{
    public class StopHandler : IHandleMessage<StopMessage>
    {
        public Task Handle(StopMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
