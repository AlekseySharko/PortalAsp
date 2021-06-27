using Microsoft.AspNetCore.Mvc;
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
            return Ok(CatalogContext.Manufacturers);
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

        [HttpDelete]
        public IActionResult DeleteManufacturer(long id)
        {
            ValidationResult validationResult =
                ManufacturerValidator.ValidateOnDelete(id, CatalogContext.Manufacturers);
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.Products.Remove(new Product { ProductId = id });
            CatalogContext.SaveChanges();
            return Ok();
        }
    }
}
