using System.Collections.Generic;
using PortalModels.Catalog.Products;

namespace PortalModels.Catalog.CatalogCategories
{ 
    public class CatalogMainCategory
    {
        public long CatalogMainCategoryId { get; set; }
        public string Name { get; set; }
        public string ImageAddress { get; set; }
        public List<CatalogSubCategory> SubCategories { get; set; }
    }
}
