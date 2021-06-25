using System.Linq;
using PortalModels.Catalog.CatalogCategories;

namespace PortalAsp.Controllers.Helpers.Catalog
{
    public class CatalogSubCategoryValidator
    {
        public static ValidationResult ValidateOnAdd(CatalogSubCategory subCategory,
            IQueryable<CatalogSubCategory> existingCategories)
        {
            ValidationResult result = new ValidationResult
            {
                IsValid = true,
                Message = ""
            };

            if (subCategory is null)
            {
                result.IsValid = false;
                result.Message += "Subcategory is null. ";
                return result;
            }

            if (string.IsNullOrWhiteSpace(subCategory.Name))
            {
                result.IsValid = false;
                result.Message += "Name is invalid. ";
                return result;
            }

            if (subCategory.CatalogSubCategoryId != 0)
            {
                result.IsValid = false;
                result.Message += "CatalogSubCategoryId has to be 0 or undefined. ";
            }

            if (subCategory.ProductCategories != null && subCategory.ProductCategories.Count > 0)
            {
                result.IsValid = false;
                result.Message += "Subcategory can't have any product categories now. ";
            }

            if (subCategory.ParentMainCategory != null)
            {
                result.IsValid = false;
                result.Message += "Subcategory's parent main category has to be undefined. ";
            }

            if (existingCategories.Contains(subCategory, new CaseInsensitiveNameComparer<CatalogSubCategory>()))
            {
                result.IsValid = false;
                result.Message += "Such subcategory already exists. ";
            }

            return result;
        }
    }
}
