﻿namespace Ordering.Domain.Models
{
    public class Product : Entity<ProductId>
    {

        public string Name { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;

        //Rich Domain Model
        public static Product Create(ProductId id, string name, decimal price)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));
            
            var product = new Product
            {
                Id = id,
                Name = name,
                Price = price,
                CreatedAt = DateTime.UtcNow
            };
            return product;
        }




    }
}
