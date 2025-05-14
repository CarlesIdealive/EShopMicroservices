namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
    public record GetProductByCategoryResult(IEnumerable<Product> Products);


    internal class GetProductByCategoryHandler(
        IDocumentSession session
    ) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var products = await session.Query<Product>()
                    .Where(x => x.Categories.Contains(query.Category))
                    .ToListAsync(cancellationToken);
                return new GetProductByCategoryResult(products); // Conversión to DTO  
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting product by category", ex);
            }
        }
    }
}
