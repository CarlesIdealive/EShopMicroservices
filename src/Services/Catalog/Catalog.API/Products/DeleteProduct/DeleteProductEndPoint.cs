namespace Catalog.API.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Id);
    public record DeleteProductResponse(bool IsSuccess);


    public class DeleteProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id:guid}",
                async (Guid id, ISender sender) =>
                {
                    var result = await sender.Send(new DeleteProductCommand(id));
                    var response = result.Adapt<DeleteProductResponse>();
                    return result.IsSuccess
                        ? Results.Ok(response)
                        : Results.NotFound(response);
                })
                .WithName("DeleteProduct")
                .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
                .Produces<DeleteProductResponse>(StatusCodes.Status404NotFound)
                .WithTags("Products")
                .WithDescription("Delete a product by its ID");

        }
    }
}
