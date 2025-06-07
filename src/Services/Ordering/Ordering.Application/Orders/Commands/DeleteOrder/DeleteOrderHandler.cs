namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderHandler(IApplicationDbContext dbContext)
        : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {

        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.OrderId.Value);
            var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);
            if (order is null)
            {
                // Order not found, return failure result.
                throw new OrderNotFoundException(id: command.OrderId.Value);
            }

            dbContext.Orders.Remove(order);
            var result = await dbContext.SaveChangesAsync(cancellationToken);
            return new DeleteOrderResult(result > 0);

        }


    }
}
