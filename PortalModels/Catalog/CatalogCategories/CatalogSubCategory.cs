using PortalModels.Catalog.Products;

namespace PortalModels.Catalog.CatalogCategories
{
    public class CatalogSubCategory
    {
        public long CatalogSubCategoryId { get; set; }
        public string Name { get; set; }
        public ProductCategory[] ProductCategories { get; set; }
    }
}
