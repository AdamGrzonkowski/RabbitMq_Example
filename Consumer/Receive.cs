using Helpers.Managers;
using Helpers.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer
{
    public class Receive : MessageQueueConsumer, IReceive
    {
        protected override void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body;
            var message = BinarySerializer.ByteArrayToObject(body).ToString();
            Console.WriteLine(" [x] Received {0}", message);

            int dots = message.Split('.').Length - 1;
            Thread.Sleep(dots * 1000); // it's just here to check if RabbitMQ works correctly for multiple consumers

            Console.WriteLine(" [x] Done");

            base.Consumer_Received(sender, e);
        }
    }
}
