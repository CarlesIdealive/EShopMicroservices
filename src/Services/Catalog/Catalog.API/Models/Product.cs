namespace Catalog.API.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public List<string> Categories { get; set; } = [];
        public string Description { get; set; } = default!;
        public string ImageFile { get; set; } = default!;
        public decimal Price { get; set; }

    }
}
