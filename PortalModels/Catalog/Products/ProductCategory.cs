using System.Collections.Generic;
using PortalModels.Catalog.CatalogCategories;

namespace PortalModels.Catalog.Products
{
    public class ProductCategory : INameAware
    {
        public long ProductCategoryId { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        public CatalogSubCategory ParentSubCategory { get; set; }
    }
}
