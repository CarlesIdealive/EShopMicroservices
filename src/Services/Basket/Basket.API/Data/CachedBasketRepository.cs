namespace Basket.API.Data
{
    public class CachedBasketRepository
        (IBasketRepository basketRepository, IDistributedCache cache) 
        : IBasketRepository
    {

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            // check if the basket is in the cache
            var chachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(chachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(chachedBasket)!;
            // if not found in cache, get it from the database
            var basket = await basketRepository.GetBasket(userName, cancellationToken);
            // if found in the database, set it in the cache
            if (basket != null)
            {
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));
                await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), options, cancellationToken);
            }
            return basket;
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await basketRepository.UpdateBasket(basket, cancellationToken);
            // set the basket in the cache
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), options, cancellationToken);
            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await basketRepository.DeleteBasket(userName, cancellationToken);
            // remove the basket from the cache
            await cache.RemoveAsync(userName, cancellationToken);
            return true;
        }

    }
}
