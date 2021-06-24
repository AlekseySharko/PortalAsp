using System.Collections.Generic;
using PortalModels.Catalog.Products;

namespace PortalModels.Catalog.CatalogCategories
{
    public class CatalogSubCategory : INameAware
    {
        public long CatalogSubCategoryId { get; set; }
        public string Name { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public CatalogMainCategory ParentMainCategory { get; set; }
    }
}
