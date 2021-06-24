namespace PortalModels.Catalog.Products
{ 
    public class Manufacturer : INameAware
    {
        public long ManufacturerId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
