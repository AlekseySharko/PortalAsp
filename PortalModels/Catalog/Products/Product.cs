using System.Collections.Generic;

namespace PortalModels.Catalog.Products
{
    public class Product : INameAware
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public long Popularity { get; set; }
        public List<ProductImage> Images { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public ProductCategory Category { get; set; }
    }
}
