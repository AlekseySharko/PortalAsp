using Microsoft.AspNetCore.Mvc;
using PortalAsp.EfCore.Catalog;

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
    }
}
