namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger)
    : INotificationHandler<OrderUpdatedDomainEvent>
{
    public Task Handle(OrderUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Order Updated Event Handled: {DomainEvent}", notification.GetType());
        return Task.CompletedTask;

    }
}
