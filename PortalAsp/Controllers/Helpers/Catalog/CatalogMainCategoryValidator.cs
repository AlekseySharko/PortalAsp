using System.IO;
using System.Linq;
using PortalModels.Catalog.CatalogCategories;

namespace PortalAsp.Controllers.Helpers.Catalog
{
    public class CatalogMainCategoryValidator
    {
        public static ValidationResult ValidateOnAdd(CatalogMainCategory mainCategory, IQueryable<CatalogMainCategory> existingMainCategories)
        {
            ValidationResult result = new ValidationResult
            {
                IsValid = true,
                Message = ""
            };

            if (mainCategory is null)
            {
                result.IsValid = false;
                result.Message += "Main category is null. ";
                return result;
            }

            if (string.IsNullOrWhiteSpace(mainCategory.Name))
            {
                result.IsValid = false;
                result.Message += "Name is invalid. ";
                return result;
            }
            
            if (string.IsNullOrWhiteSpace(mainCategory.ImageAddress) || 
                !ImageAddressValidator.CheckExtension(Path.GetExtension(mainCategory.ImageAddress)))
            {
                result.IsValid = false;
                result.Message += "ImageAddress is invalid. ";
            }

            if (mainCategory.CatalogMainCategoryId != 0)
            {
                result.IsValid = false;
                result.Message += "CatalogMainCategoryId should be 0 or undefined. ";
            }

            if (mainCategory.SubCategories != null && mainCategory.SubCategories.Count > 0)
            {
                result.IsValid = false;
                result.Message += "Main category can't have any subcategories now. ";
            }

            if (existingMainCategories.Contains(mainCategory, new CaseInsensitiveNameComparer<CatalogMainCategory>()))
            {
                result.IsValid = false;
                result.Message += "Such main category already exists. ";
            }

            return result;
        }
    }
}
