namespace PortalModels.Catalog.Products
{
    public class Product
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public Image[] Images { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public ProductCategory Category { get; set; }
    }
}
