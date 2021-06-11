namespace PortalModels.Catalog.Products
{
    public class ProductCategory
    {
        public long ProductCategoryId { get; set; }
        public string Name { get; set; }
        public Product[] Products { get; set; }
    }
}
