using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {


        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();
            if (await session.Query<Product>().AnyAsync(cancellation)) return;

            session.Store<Product>(GetPreconfiguredProducts());
            await session.SaveChangesAsync(cancellation);
        }


        private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "IPhone X",
                Description = "Description 1",
                Price = 950.00M,
                ImageFile = "product-1.jpg",
                Categories = new List<string> { "Smart Phone" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Samsung 10",
                Description = "Description 2",
                Price = 20.99m,
                ImageFile = "product-2.jpg",
                Categories = new List<string> { "Smart Phone" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Google Pixel",
                Description = "Description 3",
                Price = 950.00M,
                ImageFile = "product-3.jpg",
                Categories = new List<string> { "Smart Phone" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Nokia X",
                Description = "Description 4",
                Price = 950.00M,
                ImageFile = "product-4.jpg",
                Categories = new List<string> { "Smart Phone" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Sony X",
                Description = "Description 5",
                Price = 950.00M,
                ImageFile = "product-5.jpg",
                Categories = new List<string> { "Smart Phone" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "LG X",
                Description = "Description 6",
                Price = 950.00M,
                ImageFile = "product-6.jpg",
                Categories = new List<string> { "Smart Phone" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "IPhone 11",
                Description = "Description 7",
                Price = 950.00M,
                ImageFile = "product-7.jpg",
                Categories = new List<string> { "Smart Phone" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "IPhone 12",
                Description = "Description 8",
                Price = 950.00M,
                ImageFile = "product-8.jpg",
                Categories = new List<string> { "Smart Phone" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "IPhone 13",
                Description = "Description 9",
                Price = 950.00M,
                ImageFile = "product-9.jpg",
                Categories = new List<string> { "Smart Phone" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "IPhone 14",
                Description = "Description 10",
                Price = 950.00M,
                ImageFile = "product-10.jpg",
                Categories = new List<string> { "Smart Phone" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "IPhone 15",
                Description = "Description 11",
                Price = 950.00M,
                ImageFile = "product-11.jpg",
                Categories = new List<string> { "Smart Phone" }
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "IPhone 16",
                Description = "Description 12",
                Price = 950.00M,
                ImageFile = "product-12.jpg",
                Categories = new List<string> { "Smart Phone" }
            }
        };




    }


}
