using Microsoft.AspNetCore.Mvc;
using PortalAsp.EfCore.Catalog;

namespace PortalAsp.Controllers.Catalog.Products
{
    [Route("api/catalog/product-categories")]
    public class ProductCategoryController : Controller
    {
        public CatalogContext CatalogContext { get; set; }
        public ProductCategoryController(CatalogContext catalogContext) => CatalogContext = catalogContext;

        [HttpGet]
        public IActionResult GetProductCategories()
        {
            return Ok(CatalogContext.ProductCategories);
        }
    }
}
