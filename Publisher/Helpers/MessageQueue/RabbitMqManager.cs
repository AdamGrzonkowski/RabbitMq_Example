using Helpers.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

public class RabbitMqManager : IRabbitMqManager
{
    /// <summary>
    /// Identifier of the queue.
    /// </summary>
    protected string QueueName => "task5";

    /// <summary>
    /// Host name of rabbit mq server.
    /// </summary>
    private string _hostName => "localhost";

    private bool _consumerExclusiveAccess => true;

    private readonly IConnection _conn; // declaring connection at class level variable allows it to be easily reused

    protected static readonly object ConnectionLock = new object();


    public RabbitMqManager()
    {
        _conn = OpenConnection();
    }

    protected IModel GetOpenChannel()
    {
        IModel channel = null;
        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName
            };

            channel = _conn.CreateModel();
            channel.QueueDeclare(queue: QueueName,
                                    durable: true, // the queue will survive a broker restart (for example RabbitMQ server crush)
                                    exclusive: _consumerExclusiveAccess, // used by only one connection and the queue will be deleted when that connection closes
                                    autoDelete: false, // queue that has had at least one consumer is deleted when last consumer unsubscribes
                                    arguments: null); // optional; used by plugins and broker-specific features such as message TTL, queue length limit, etc

            return channel;         
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error while establiching connection to the channel.");
            Console.WriteLine(ex);
            throw ex;
        }
    }

    private IConnection OpenConnection()
    {
        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName
            };

            return factory.CreateConnection();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while establishing connection to the queue.");
            Console.WriteLine(ex);
            throw ex;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_conn == null)
            {
                return;
            }
            lock (ConnectionLock)
            {
                if (_conn == null)
                {
                    return;
                }
                _conn?.Dispose();
            }
        }
    }
}
