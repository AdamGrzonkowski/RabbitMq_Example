using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.MessageQueue
{
    public interface IMessageQueueProducer : IRabbitMqManager
    {
        void SendMessage(object message);
    }
}
