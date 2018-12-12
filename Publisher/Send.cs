using Helpers.MessageQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    public class Send : ISend
    {
        public void SendMessage(string msg)
        {
            using (IMessageQueueProducer mq = new MessageQueueProducer())
            {
                mq.SendMessage(msg);
            }
        }
    }
}
