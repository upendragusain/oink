using RabbitMQ.Client;
using Serilog;
using System;

namespace EventBusRabbitMQ
{
    //TODO: add Poly retry logic
    //TODO: attach rabbitMQ connection event handlers
    public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        IConnection _connection;
        bool _disposed;

        object sync_root = new object();

        public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool IsConnected => _connection != null
            && _connection.IsOpen
            && !_disposed;

        public bool TryConnect()
        {
            Log.Information("RabbitMQ Client is trying to connect");

            //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/lock-statement
            lock (sync_root)
            {
                _connection = _connectionFactory.CreateConnection();

                if (IsConnected)
                {
                    Log.Information("RabbitMQ Client acquired a persistent connection to '{HostName}' " +
                        "and is subscribed to failure events", 
                        _connection.Endpoint.HostName);

                    return true;
                }
                else
                {
                    Log.Error("FATAL ERROR: RabbitMQ connections could not be created and opened");
                    return false;
                }
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                Log.Error("attempted to create a channel on a non existent connection");
                throw new Exception("No RabbitMQ connections are available to create a channel");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (Exception ex)
            {
                //swallow it?
                Log.Error("Error disposing connection: {0}", ex);
            }
        }
    }
}
