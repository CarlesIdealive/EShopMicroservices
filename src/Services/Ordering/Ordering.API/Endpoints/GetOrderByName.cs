
using Ordering.Application.Orders.Queries.GetOrderByName;

namespace Ordering.API.Endpoints;

//Se utilizan los datos del Request parametro para crear un Query que se envía al Mediator, que a su vez llama al Handler correspondiente
//public record GetOrdersByNameRequest(string Name);
public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);


public class GetOrderByName : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/name/{name}", async (string name, ISender sender) =>
        {
            var query = new GetOrdersByNameQuery(name);
            var result = await sender.Send(query);
            var response = result.Adapt<GetOrdersByNameResponse>();
            return Results.Ok(response);
        })
        .WithName("GetOrderByName")
        .WithSummary("Get orders by name")
        .WithDescription("Retrieves orders based on the provided name.")
        .Produces<GetOrdersByNameResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);



    }
}
