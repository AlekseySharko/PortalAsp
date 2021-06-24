using System.Collections.Generic;

namespace PortalModels.Catalog.CatalogCategories
{ 
    public class CatalogMainCategory : INameAware
    {
        public long CatalogMainCategoryId { get; set; }
        public string Name { get; set; }
        public string ImageAddress { get; set; }
        public List<CatalogSubCategory> SubCategories { get; set; }
    }
}
