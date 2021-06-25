using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalAsp.Controllers.Helpers;
using PortalAsp.Controllers.Helpers.Catalog;
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

        public IActionResult PostProduct([FromBody] Product product, [FromQuery] long productCategoryId)
        {
            ProductCategory productCategory = CatalogContext.ProductCategories
                .Include(pc => pc.Products)
                .FirstOrDefault(pc => pc.ProductCategoryId == productCategoryId);
            if (productCategory is null)
                return BadRequest("No such subcategory or no subcategory id is provided");

            ValidationResult validationResult =
                ProductValidator.ValidateOnAdd(product, CatalogContext.Products, CatalogContext.Manufacturers);
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            return Ok();
        }
    }
}
