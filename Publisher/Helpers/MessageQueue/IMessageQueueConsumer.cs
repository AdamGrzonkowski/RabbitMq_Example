using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Managers
{
    public interface IMessageQueueConsumer : IRabbitMqManager
    {
        void ReceiveMessage(EventHandler<BasicDeliverEventArgs> receiveHandler);
    }
}
