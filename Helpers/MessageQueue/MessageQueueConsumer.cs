using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Helpers.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Helpers.Managers
{
    public class MessageQueueConsumer : RabbitMqManager, IMessageQueueConsumer
    {
        private Lazy<IModel> _channel => new Lazy<IModel>(GetOpenChannel);
        private IModel _consumeChannel => _channel.Value;

        public void ReceiveMessage()
        {
            _consumeChannel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false); // ensures round-robin (Work-Queue)

            var consumer = new EventingBasicConsumer(_consumeChannel);
            consumer.Received += Consumer_Received;

            _consumeChannel.BasicConsume(queue: QueueName, // the name of the queue
                                    autoAck: false, // true if the server should consider messages acknowledged once delivered; false if the server should expect explicit acknowledgements
                                    consumer: consumer); // an interface to the consumer object
        }

        protected virtual void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            _consumeChannel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false); // makes sure we send ack flag after entire receive logic has been processed. In other case, message is re-queued
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _consumeChannel != null)
            {
                lock (ConnectionLock)
                {
                    if (_consumeChannel != null)
                    {
                        if (!_consumeChannel.IsClosed)
                        {
                            _consumeChannel.Close(200, "StopConsuming");
                        }
                        _consumeChannel?.Dispose();
                    }
                }
            }
            base.Dispose(disposing);
        }
    }
}
