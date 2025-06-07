namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandler (ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedDomainEvent>
{


    public Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Order Created Event Handled: {DomainEvent}", notification.GetType());
        return Task.CompletedTask;
    }


}
