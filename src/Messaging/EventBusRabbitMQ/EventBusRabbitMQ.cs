using Autofac;
using EventBus;
using EventBus.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ
{
    //TODO: add poly retry logic
    public class EventBusRabbitMQ : IEventBus
    {
        private readonly IRabbitMQPersistentConnection _rabbitMQPersistentConnection;
        private readonly string _queueName;
        const string BROKER_NAME = "oink_event_bus";
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "oink_event_bus";

        private IModel _consumerChannel;
        public EventBusRabbitMQ(IRabbitMQPersistentConnection rabbitMQPersistentConnection,
            ILifetimeScope autofac,
            string queueName)
        {
            _rabbitMQPersistentConnection = rabbitMQPersistentConnection;
            _queueName = queueName;
            _autofac = autofac;

            _consumerChannel = CreateConsumerChannel();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_rabbitMQPersistentConnection.IsConnected)
            {
                _rabbitMQPersistentConnection.TryConnect();
            }

            Log.Information("Creating RabbitMQ consumer channel");

            var channel = _rabbitMQPersistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: BROKER_NAME,
                                    type: "direct");

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            return channel;
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!_rabbitMQPersistentConnection.IsConnected)
                _rabbitMQPersistentConnection.TryConnect();

            var eventName = @event.GetType().Name;

            Log.Information("Creating RabbitMQ channel to publish event: {EventId} ({EventName})",
                @event.Id, eventName);

            using (var channel = _rabbitMQPersistentConnection.CreateModel())
            {
                Log.Information("Declaring RabbitMQ exchange to publish event: {EventId}",
                    @event.Id);

                channel.ExchangeDeclare(exchange: BROKER_NAME,
                    type: ExchangeType.Direct);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                Log.Information("Publishing event to RabbitMQ: {EventId}",
                    @event.Id);

                //The routing algorithm behind a direct exchange is simple - a message goes to the queues 
                //whose binding key exactly matches the routing key of the message.
                channel.BasicPublish(exchange: BROKER_NAME,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name;

            Log.Information("Subscribing to event {EventName} with {EventHandler}",
                eventName, typeof(TH).GetGenericTypeName());

            if (!_rabbitMQPersistentConnection.IsConnected)
                _rabbitMQPersistentConnection.TryConnect();

            using (var channel = _rabbitMQPersistentConnection.CreateModel())
            {
                Log.Information("Binding to queue: {0} with routingKey {1}",
                    _queueName, eventName);

                channel.QueueBind(queue: _queueName,
                    exchange: BROKER_NAME,
                    routingKey: eventName);

                Log.Information("Waiting for messages.");
            }

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += ProcessEvent<T, TH>;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                Log.Error("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name;

            Log.Information("Unsubscribing to event {EventName} with {EventHandler}",
                eventName, typeof(TH).GetGenericTypeName());

            using (var channel = _rabbitMQPersistentConnection.CreateModel())
            {
                channel.QueueUnbind(queue: _queueName,
                    exchange: BROKER_NAME,
                    routingKey: eventName);
            }
        }

        private async Task ProcessEvent<T, TH>(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            Log.Information("Processing RabbitMQ event: {EventName}", eventName);

            using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
            {
                var handler = scope.ResolveOptional(typeof(TH));
                var eventType = typeof(T);
                var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                await (Task)concreteType.GetMethod("Handle").Invoke(
                    handler, new object[] { integrationEvent });
            }
        }
    }
}
