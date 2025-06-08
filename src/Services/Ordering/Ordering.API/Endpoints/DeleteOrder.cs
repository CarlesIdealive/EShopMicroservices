
using Ordering.Application.Orders.Commands.DeleteOrder;
using Ordering.Domain.ValueObjects;

namespace Ordering.API.Endpoints;

//public record DeleteOrderRequest(Guid OrderId);
public record DeleteOrderResponse(bool IsSuccess);



public class DeleteOrder : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("orders/{orderId}", async (Guid orderId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteOrderCommand(orderId));
            var response = result.Adapt<DeleteOrderResponse>();
            return Results.Ok(response);
        })
        .WithName("DeleteOrder")
        .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete an existing order")
        .WithDescription("Deletes an existing order in the system. The order must exist and the provided data must be valid.");

    }

}
