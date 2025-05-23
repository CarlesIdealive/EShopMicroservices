﻿
namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);



    internal class GetProductsQueryHandler(
        IDocumentSession session
    ) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var products = await session.Query<Product>()
                    .ToPagedListAsync(pageNumber: query.PageNumber ?? 1, pageSize: query.PageSize ?? 10,  cancellationToken);
                return new GetProductsResult(products); //Conversión to DTO
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting products", ex);
            }
        }
    }



}
