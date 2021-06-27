using System.Linq;
using PortalModels.Catalog.Products;

namespace PortalAsp.Controllers.Helpers.Catalog
{
    public static class ProductCategoryValidator
    {
        public static ValidationResult ValidateOnAdd(ProductCategory productCategory, IQueryable<ProductCategory> existingCategories)
        {
            ValidationResult result = new ValidationResult
            {
                IsValid = true,
                Message = ""
            };

            if (productCategory is null)
            {
                result.IsValid = false;
                result.Message += "Product category is null. ";
                return result;
            }

            if (string.IsNullOrWhiteSpace(productCategory.Name))
            {
                result.IsValid = false;
                result.Message += "Name is invalid. ";
                return result;
            }

            if (productCategory.ProductCategoryId != 0)
            {
                result.IsValid = false;
                result.Message += "ProductCategoryId should be 0 or undefined. ";
            }

            if (productCategory.Products != null && productCategory.Products.Count > 0)
            {
                result.IsValid = false;
                result.Message += "Product category can't have any products now. ";
            }

            if (existingCategories.FirstOrDefault(pc => NameComparer.CaseInsensitive(pc, productCategory)) != null)
            {
                result.IsValid = false;
                result.Message += "Such product category already exists. ";
            }

            return result;
        }
    }
}
