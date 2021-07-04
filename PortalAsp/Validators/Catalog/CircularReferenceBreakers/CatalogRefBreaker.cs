using System.Collections.Generic;
using PortalModels.Catalog.CatalogCategories;

namespace PortalAsp.Validators.Catalog.CircularReferenceBreakers
{
    public static class CatalogRefBreaker
    {
        public static void BreakSubcategoryInfiniteReferenceCircle(IEnumerable<CatalogMainCategory> mainCategories, bool breakSubcategoryTo = false)
        {
            foreach (CatalogMainCategory catalogMainCategory in mainCategories)
            {
                foreach (var catalogSubCategory in catalogMainCategory.SubCategories)
                {
                    catalogSubCategory.ParentMainCategory = null;
                }
                if(breakSubcategoryTo) BreakProductCategoryInfiniteReferenceCircle(catalogMainCategory.SubCategories);
            }
        }

        public static void BreakProductCategoryInfiniteReferenceCircle(IEnumerable<CatalogSubCategory> subCategories)
        {
            foreach (CatalogSubCategory catalogSubCategory in subCategories)
            {
                foreach (var productCategory in catalogSubCategory.ProductCategories)
                {
                    productCategory.ParentSubCategory = null;
                }
            }
        }
    }
}
