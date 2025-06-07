namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler (IApplicationDbContext dbContext)
        : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.Order.Id);
            var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);
            if (order is null)
            {
                // Order not found, return failure result.
                throw new OrderNotFoundException(command.Order.Id);
            }

            UpdateOrderWithNewValues(order, command.Order);

            dbContext.Orders.Update(order);
            var result = await dbContext.SaveChangesAsync(cancellationToken);
            return new UpdateOrderResult(result > 0);




        }

        private void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
        {
            var updatedshippingAddress = Address.Of(orderDto.ShippingAddress.FirstName,
                orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress,
                orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country,
                orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
            var updatedBillingAddres = Address.Of(orderDto.BillingAddress.FirstName,
                orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress,
                orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country,
                orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
            var updatedPayment = Payment.Of(
                orderDto.Payment.CardName,
                orderDto.Payment.CardNumber,
                orderDto.Payment.Expiration,
                orderDto.Payment.Cvv,
                orderDto.Payment.PaymentMethod
            );
            order.Update(
                orderName: OrderName.Create(orderDto.OrderName), 
                updatedshippingAddress, 
                updatedBillingAddres, 
                updatedPayment
            );




        }



    }
    
}
