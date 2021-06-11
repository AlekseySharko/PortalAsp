using PortalModels.Catalog.Products;

namespace PortalModels.Catalog.CatalogCategories
{ 
    public class CatalogMainCategory
    {
        public long CatalogMainCategoryId { get; set; }
        public string Name { get; set; }
        public Image Image { get; set; }
        public CatalogSubCategory[] SubCategories { get; set; }
    }
}
