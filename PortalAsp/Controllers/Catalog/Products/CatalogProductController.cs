using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PortalAsp.EfCore.Catalog;
using PortalModels.Catalog.Products;

namespace PortalAsp.Controllers.Catalog.Products
{
    [Route("api/catalog/products")]
    public class CatalogProductController : Controller
    {
        public CatalogContext CatalogContext { get; set; }
        public CatalogProductController(CatalogContext catalogContext) => CatalogContext = catalogContext;

        [HttpGet]
        public IActionResult GetProducts(long productCategoryId)
        {
            IEnumerable<Product> requestedProducts = CatalogContext.ProductCategories
                .First(pc => pc.ProductCategoryId == productCategoryId).Products;
            return Ok(requestedProducts);
        }

        public IActionResult AddProduct([FromBody] Product product)
        {
            return BadRequest();
        }
    }
}
