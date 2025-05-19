namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart Cart);


    internal class GetBasketQueryHandler(IBasketRepository basketRepository) 
        : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
        {
            try
            {
                //TODO: get basket from DBs
                var basket = await basketRepository.GetBasket(query.UserName, cancellationToken);
                return new GetBasketResult(basket);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting Basket User", ex);
            }
        }
    }



}
