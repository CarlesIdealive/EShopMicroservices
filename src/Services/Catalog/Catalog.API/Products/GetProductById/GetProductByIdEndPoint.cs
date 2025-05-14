namespace Catalog.API.Products.GetProductById
{
    //public record GetProuctById(Guid Id);
    public record GetProductByIdResponse(Product Product);


    public class GetProductByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            try
            {
                app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
                {
                    var result = await sender.Send(new GetProductByIdQuery(id));
                    var response = result.Adapt<GetProductByIdResponse>();
                    return Results.Ok(response);
                })
                .WithName("GetProductById")
                .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get a product by id")
                .WithDescription("Get a product by id from the catalog");

            }
            catch (Exception ex)
            {
                Results.Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }


}
