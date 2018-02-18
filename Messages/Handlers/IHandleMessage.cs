using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Handlers
{
    public interface IHandleMessage<T> where T : IMessage
    {
        Task Handle(T message);
    }
}
