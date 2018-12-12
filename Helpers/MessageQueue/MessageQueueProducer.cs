using Helpers.Serialization;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.MessageQueue
{
    public class MessageQueueProducer : RabbitMqManager, IMessageQueueProducer
    {
        public void SendMessage(object message)
        {
            var body = BinarySerializer.ObjectToByteArray(message);

            using (IModel channel = GetOpenChannel())
            {
                var props = channel.CreateBasicProperties();
                props.Persistent = true;  // saves message to the disk - in case of RabbitMQ server crush we can restore state of the queue

                channel.BasicPublish(exchange: "",
                                     routingKey: QueueName,
                                     basicProperties: props,
                                     body: body);
            }

            Console.WriteLine(" [x] Sent {0}", message);
        }
    }
}
