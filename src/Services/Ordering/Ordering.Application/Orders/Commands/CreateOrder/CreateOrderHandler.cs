namespace Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler (IApplicationDbContext dbContext)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateNewOrder(command.Order);
        await dbContext.Orders.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new CreateOrderResult(order.Id.Value);
    }


    private static Order CreateNewOrder(OrderDto orderDto)
    {
        var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName,
            orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress,
            orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country,
            orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var billingAddres = Address.Of(orderDto.BillingAddress.FirstName,
            orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress,
            orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country,
            orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);

        var newOrder = Order.Create(
            id: OrderId.Of(Guid.NewGuid()),
            customerId: CustomerId.Of(orderDto.CustomerId),
            orderName: OrderName.Create(orderDto.OrderName),
            shippingAddress: shippingAddress,
            billingAddress: billingAddres,
            payment: Payment.Of(
                orderDto.Payment.CardName,
                orderDto.Payment.CardNumber,
                orderDto.Payment.Expiration,
                orderDto.Payment.Cvv,
                orderDto.Payment.PaymentMethod
            )
        );

        foreach (var item in orderDto.OrderItems)
        {
            newOrder.AddItem(
                productId: ProductId.Create(item.ProductId),
                quantity: item.Quantity,
                price: item.Price);
        }


        return newOrder;
    }
}