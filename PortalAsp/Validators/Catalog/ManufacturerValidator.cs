using System.Linq;
using PortalModels.Catalog.Products;

namespace PortalAsp.Validators.Catalog
{
    public class ManufacturerValidator
    {
        public static ValidationResult ValidateOnAdd(Manufacturer manufacturer, IQueryable<Manufacturer> existingManufacturers)
        {
            ValidationResult result = new ValidationResult
            {
                IsValid = true,
                Message = ""
            };

            if (manufacturer is null)
            {
                result.IsValid = false;
                result.Message += "Manufacturer is null. ";
                return result;
            }

            if (string.IsNullOrWhiteSpace(manufacturer.Name))
            {
                result.IsValid = false;
                result.Message += "Name is invalid. ";
                return result;
            }

            if (string.IsNullOrWhiteSpace(manufacturer.Country))
            {
                result.IsValid = false;
                result.Message += "Country is invalid. ";
            }

            if (manufacturer.ManufacturerId != 0)
            {
                result.IsValid = false;
                result.Message += "ManufacturerId should be 0 or undefined. ";
            }

            if (existingManufacturers.FirstOrDefault(m => m.Name.ToLower() == manufacturer.Name.ToLower()) != null)
            {
                result.IsValid = false;
                result.Message += "Such manufacturer already exists. ";
            }

            return result;
        }

        public static ValidationResult ValidateOnEdit(Manufacturer manufacturer, IQueryable<Manufacturer> existingManufacturers)
        {
            ValidationResult result = new ValidationResult
            {
                IsValid = true,
                Message = ""
            };

            if (manufacturer is null)
            {
                result.IsValid = false;
                result.Message += "Manufacturer is null. ";
                return result;
            }

            if (string.IsNullOrWhiteSpace(manufacturer.Name))
            {
                result.IsValid = false;
                result.Message += "Name is invalid. ";
                return result;
            }

            if (string.IsNullOrWhiteSpace(manufacturer.Country))
            {
                result.IsValid = false;
                result.Message += "Country is invalid. ";
            }

            if (existingManufacturers.FirstOrDefault(m => m.ManufacturerId == manufacturer.ManufacturerId) == null)
            {
                result.IsValid = false;
                result.Message += "Such manufacturer doesn't exist. ";
            }

            if (existingManufacturers.FirstOrDefault(m =>
                    m.Name.ToLower() == manufacturer.Name.ToLower() &&
                    m.ManufacturerId != manufacturer.ManufacturerId) !=
                null)
            {
                result.IsValid = false;
                result.Message += "Such manufacturer already exists. ";
            }

            return result;
        }

        public static ValidationResult ValidateOnDelete(long manufacturerId, IQueryable<Manufacturer> existingManufacturers)
        {
            ValidationResult result = new ValidationResult
            {
                IsValid = true,
                Message = ""
            };

            if (existingManufacturers.FirstOrDefault(m => m.ManufacturerId == manufacturerId) == null)
            {
                result.IsValid = false;
                result.Message = "Such manufacturer doesn't exist. ";
            }

            return result;
        }
    }
}
