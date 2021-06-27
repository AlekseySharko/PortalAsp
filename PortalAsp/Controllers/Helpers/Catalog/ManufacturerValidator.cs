using System.Linq;
using PortalModels.Catalog.Products;

namespace PortalAsp.Controllers.Helpers.Catalog
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

            if (existingManufacturers.FirstOrDefault(m => NameComparer.CaseInsensitive(m, manufacturer)) != null)
            {
                result.IsValid = false;
                result.Message += "Such manufacturer already exists. ";
            }

            return result;
        }
    }
}
