using Basket.API.IntegrationEvents.Events;
using Basket.API.Repositories;
using EventBus.Events;
using Serilog;
using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.API.IntegrationEvents.EventHandling
{
    public class UserCheckoutAcceptedIntegrationEventHandler : IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>
    {
        private readonly IBasketRepository _repository;

        public UserCheckoutAcceptedIntegrationEventHandler(
            IBasketRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(UserCheckoutAcceptedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                Log.Information("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})",
                    @event.Id, Program.AppName, @event);

                //simulate time taken for order processing
                Thread.Sleep(TimeSpan.FromMinutes(1));

                await _repository.DeleteBasketAsync(@event.UserId.ToString());
            }
        }
    }
}
