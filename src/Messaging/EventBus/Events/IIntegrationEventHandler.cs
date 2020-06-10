using System.Threading.Tasks;

namespace EventBus.Events
{
    // TODO: add contravariance support (in)
    public interface IIntegrationEventHandler<TIntegrationEvent>
        where TIntegrationEvent: IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}
