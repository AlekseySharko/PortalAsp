using System.Linq;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Products;

namespace PortalModels.Validators.Catalog
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

            if (productCategory.ParentSubCategory != null)
            {
                result.IsValid = false;
                result.Message += "Parent sub category must be null on creation. ";
            } 

            if (existingCategories.FirstOrDefault(pc => pc.Name.ToLower() == productCategory.Name.ToLower()) != null)
            {
                result.IsValid = false;
                result.Message += "Such product category already exists. ";
            }

            return result;
        }

        public static ValidationResult ValidateOnEdit(ProductCategory productCategory,
            IQueryable<ProductCategory> existingCategories, IQueryable<CatalogSubCategory> existingSubCategories)
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

            if (existingCategories.FirstOrDefault(pc => pc.ProductCategoryId == productCategory.ProductCategoryId) ==
                null)
            {
                result.IsValid = false;
                result.Message += "Such product category doesn't exist. ";
            }

            if (productCategory.Products != null && productCategory.Products.Count > 0)
            {
                result.IsValid = false;
                result.Message += "Product category can't have any products now. ";
            }

            if (productCategory.ParentSubCategory == null || existingSubCategories.FirstOrDefault(sc =>
                sc.CatalogSubCategoryId == productCategory.ParentSubCategory.CatalogSubCategoryId) == null)
            {
                result.IsValid = false;
                result.Message += "Parent sub category null or doesn't exist. ";
            }

            if (existingCategories.FirstOrDefault(pc =>
                pc.Name.ToLower() == productCategory.Name.ToLower() &&
                pc.ProductCategoryId != productCategory.ProductCategoryId) != null)
            {
                result.IsValid = false;
                result.Message += "Such product category already exists. ";
            }

            return result;
        }

        public static ValidationResult ValidateOnDelete(ProductCategory productCategory,
            IQueryable<ProductCategory> existingCategories)
        {
            ValidationResult result = new ValidationResult
            {
                IsValid = true,
                Message = ""
            };

            if (existingCategories.FirstOrDefault(pc => pc.ProductCategoryId == productCategory.ProductCategoryId) == null)
            {
                result.IsValid = false;
                result.Message += "Such product category doesn't exist. ";
            }

            return result;
        }
    }
}
