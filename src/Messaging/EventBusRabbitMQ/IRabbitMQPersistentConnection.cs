using RabbitMQ.Client;
using System;

namespace EventBusRabbitMQ
{
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        //interface field member! (readonly)
        bool IsConnected { get; }

        bool TryConnect();

        //used to create channel from the singleton connection
        IModel CreateModel();

    }
}
