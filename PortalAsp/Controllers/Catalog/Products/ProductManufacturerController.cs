using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalAsp.Controllers.Helpers;
using PortalAsp.Controllers.Helpers.Catalog;
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
        public IActionResult PostManufacturer([FromBody] Manufacturer manufacturer)
        {
            ValidationResult validationResult =
                ManufacturerValidator.ValidateOnAdd(manufacturer, CatalogContext.Manufacturers);
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.Manufacturers.Add(manufacturer);
            CatalogContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult PutManufacturer([FromBody] Manufacturer manufacturer)
        {
            ValidationResult validationResult =
                ManufacturerValidator.ValidateOnEdit(manufacturer, CatalogContext.Manufacturers.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.Manufacturers.Update(manufacturer);
            CatalogContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteManufacturer([FromRoute]long id)
        {
            ValidationResult validationResult =
                ManufacturerValidator.ValidateOnDelete(id, CatalogContext.Manufacturers.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.Manufacturers.Remove(new Manufacturer { ManufacturerId = id });
            CatalogContext.SaveChanges();
            return Ok();
        }
    }
}
