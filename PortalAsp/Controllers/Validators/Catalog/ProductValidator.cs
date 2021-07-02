using System.Linq;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Products;

namespace PortalAsp.Controllers.Validators.Catalog
{
    public class ProductValidator
    {
        public static ValidationResult ValidateOnAdd(Product product, IQueryable<Product> existingProducts,
            IQueryable<Manufacturer> existingManufacturers)
        {
            ValidationResult result = new ValidationResult
            {
                IsValid = true,
                Message = ""
            };

            if (product is null)
            {
                result.IsValid = false;
                result.Message += "Product is null. ";
                return result;
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                result.IsValid = false;
                result.Message += "Name is invalid. ";
                return result;
            }

            if (existingProducts.FirstOrDefault(p=> p.Name.ToLower() == product.Name.ToLower()) != null)
            {
                result.IsValid = false;
                result.Message += "There's a product with the same name. ";
            }

            if (product.ProductId != 0)
            {
                result.IsValid = false;
                result.Message += "ProductCategoryId should be 0 or undefined. ";
            }

            if (string.IsNullOrWhiteSpace(product.ShortDescription))
            {
                result.IsValid = false;
                result.Message += "ShortDescription is invalid. ";
            }

            if (product.Price < 0.01M)
            {
                result.IsValid = false;
                result.Message += "Price is invalid. ";
            }

            if (product.Popularity != 0)
            {
                result.IsValid = false;
                result.Message += "Popularity must be 0. ";
            }

            if (product.Images == null || product.Images.Count < 1)
            {
                result.IsValid = false;
                result.Message += "A product must have at least one image";
            }

            if (existingManufacturers.FirstOrDefault(m => m.ManufacturerId == product.Manufacturer.ManufacturerId) == null)
            {
                result.IsValid = false;
                result.Message += "Such manufacturer doesn't exist";
            }

            if (product.Category != null)
            {
                result.IsValid = false;
                result.Message += "Product category must be null";
            }

            return result;
        }

        public static ValidationResult ValidateOnEdit(Product product, IQueryable<Product> existingProducts,
            IQueryable<Manufacturer> existingManufacturers, IQueryable<CatalogSubCategory> existingSubCategories)
        {
            ValidationResult result = new ValidationResult
            {
                IsValid = true,
                Message = ""
            };

            if (product is null)
            {
                result.IsValid = false;
                result.Message += "Product is null. ";
                return result;
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                result.IsValid = false;
                result.Message += "Name is invalid. ";
                return result;
            }

            if (existingProducts.FirstOrDefault(p => p.ProductId == product.ProductId) == null)
            {
                result.IsValid = false;
                result.Message += "There's no such product. ";
            }

            if (existingProducts.FirstOrDefault(p =>
                p.Name.ToLower() == product.Name.ToLower() && p.ProductId != product.ProductId) != null)
            {
                result.IsValid = false;
                result.Message += "There's a product with the same name. ";
            }

            if (string.IsNullOrWhiteSpace(product.ShortDescription))
            {
                result.IsValid = false;
                result.Message += "ShortDescription is invalid. ";
            }

            if (product.Price < 0.01M)
            {
                result.IsValid = false;
                result.Message += "Price is invalid. ";
            }

            if (product.Popularity != 0)
            {
                result.IsValid = false;
                result.Message += "Popularity must be 0. ";
            }

            if (product.Images == null || product.Images.Count < 1)
            {
                result.IsValid = false;
                result.Message += "A product must have at least one image";
            }

            if (existingManufacturers.FirstOrDefault(m => m.ManufacturerId == product.Manufacturer.ManufacturerId) == null)
            {
                result.IsValid = false;
                result.Message += "Such manufacturer doesn't exist";
            }

            if (existingSubCategories.FirstOrDefault(sc => sc.CatalogSubCategoryId == product.Category.ProductCategoryId) == null)
            {
                result.IsValid = false;
                result.Message += "No such subcategory";
            }

            return result;
        }

        public static ValidationResult ValidateOnDelete(Product product, IQueryable<Product> existingProducts)
        {
            ValidationResult result = new ValidationResult
            {
                IsValid = true,
                Message = ""
            };

            if (existingProducts.FirstOrDefault(p => p.ProductId == product.ProductId) == null)
            {
                result.IsValid = false;
                result.Message += "There's no such product. ";
            }

            return result;
        }
    }
}
