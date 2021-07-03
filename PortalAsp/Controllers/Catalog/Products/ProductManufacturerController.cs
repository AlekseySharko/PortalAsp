using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalAsp.Controllers.Validators;
using PortalAsp.Controllers.Validators.Catalog;
using PortalAsp.EfCore.Catalog;
using PortalModels.Catalog.Products;

namespace PortalAsp.Controllers.Catalog.Products
{
    [Route("api/catalog/product-manufacturers")]
    public class ProductManufacturerController : Controller
    {
        public CatalogContext CatalogContext { get; set; }
        public ProductManufacturerController(CatalogContext catalogContext) => CatalogContext = catalogContext;

        [HttpGet]
        public IActionResult GetManufacturers()
        {
            return Ok(CatalogContext.Manufacturers.OrderBy(m => m.Name));
        }

        [HttpPost]
        public async Task<IActionResult> PostManufacturer([FromBody] Manufacturer manufacturer)
        {
            ValidationResult validationResult =
                ManufacturerValidator.ValidateOnAdd(manufacturer, CatalogContext.Manufacturers);
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.Manufacturers.Add(manufacturer);
            await CatalogContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutManufacturer([FromBody] Manufacturer manufacturer)
        {
            ValidationResult validationResult =
                ManufacturerValidator.ValidateOnEdit(manufacturer, CatalogContext.Manufacturers.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.Manufacturers.Update(manufacturer);
            await CatalogContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManufacturer([FromRoute]long id)
        {
            ValidationResult validationResult =
                ManufacturerValidator.ValidateOnDelete(id, CatalogContext.Manufacturers.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.Manufacturers.Remove(new Manufacturer { ManufacturerId = id });
            await CatalogContext.SaveChangesAsync();
            return Ok();
        }
    }
}
