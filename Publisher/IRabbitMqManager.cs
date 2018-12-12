using System;

public interface IRabbitMqManager
{
    void SendMessage(object message);
    object ReceiveMessage();
}
