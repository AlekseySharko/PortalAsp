using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalAsp.Controllers.Helpers;
using PortalAsp.Controllers.Helpers.Catalog;
using PortalAsp.EfCore.Catalog;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Products;

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

        [HttpPost]
        public IActionResult PostProductCategory([FromBody] ProductCategory productCategory,
            [FromQuery] long subCategoryId)
        {
            CatalogSubCategory subCategory = CatalogContext.CatalogSubCategories
                .Include(sc => sc.ProductCategories)
                .FirstOrDefault(sc => sc.CatalogSubCategoryId == subCategoryId);

            if (subCategory is null)
                return BadRequest("No such subcategory or no subcategory id is provided");

            ValidationResult validationResult =
                ProductCategoryValidator.ValidateOnAdd(productCategory, CatalogContext.ProductCategories);
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            subCategory.ProductCategories ??= new List<ProductCategory>();
            subCategory.ProductCategories.Add(productCategory);
            CatalogContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult PutProductCategory([FromBody] ProductCategory productCategory)
        {
            ValidationResult validationResult =
                ProductCategoryValidator.ValidateOnEdit(productCategory, CatalogContext.ProductCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);
            
            CatalogContext.ProductCategories.Update(productCategory);
            CatalogContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteProductCategory([FromQuery] long id)
        {
            ProductCategory productCategory = new ProductCategory {ProductCategoryId = id};

            ValidationResult validationResult =
                ProductCategoryValidator.ValidateOnDelete(productCategory, CatalogContext.ProductCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.ProductCategories.Remove(productCategory);
            CatalogContext.SaveChanges();
            return Ok();
        }
    }
}
