using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalAsp.Controllers.Validators;
using PortalAsp.Controllers.Validators.Catalog;
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
        public async Task<IActionResult> PostProductCategory([FromBody] ProductCategory productCategory,
            [FromQuery] long subCategoryId)
        {
            CatalogSubCategory subCategory = await CatalogContext.CatalogSubCategories
                .Include(sc => sc.ProductCategories)
                .FirstOrDefaultAsync(sc => sc.CatalogSubCategoryId == subCategoryId);

            if (subCategory is null)
                return BadRequest("No such subcategory or no subcategory id is provided");

            ValidationResult validationResult =
                ProductCategoryValidator.ValidateOnAdd(productCategory, CatalogContext.ProductCategories);
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            subCategory.ProductCategories ??= new List<ProductCategory>();
            subCategory.ProductCategories.Add(productCategory);
            await CatalogContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutProductCategory([FromBody] ProductCategory productCategory)
        {
            ValidationResult validationResult =
                ProductCategoryValidator.ValidateOnEdit(productCategory,
                    CatalogContext.ProductCategories.AsNoTracking(),
                    CatalogContext.CatalogSubCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await EditProductCategory(productCategory);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory([FromRoute] long id)
        {
            ProductCategory productCategory = new ProductCategory {ProductCategoryId = id};

            ValidationResult validationResult =
                ProductCategoryValidator.ValidateOnDelete(productCategory, CatalogContext.ProductCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.ProductCategories.Remove(productCategory);
            await CatalogContext.SaveChangesAsync();
            return Ok();
        }

        private async Task EditProductCategory(ProductCategory productCategory)
        {
            ProductCategory existingProductCategory = CatalogContext.ProductCategories.FirstOrDefault(pc =>
                pc.ProductCategoryId == productCategory.ProductCategoryId);
            if (existingProductCategory == null) throw new Exception("No such product category");

            existingProductCategory.Name = productCategory.Name;
            CatalogSubCategory parentSubCategory = CatalogContext.CatalogSubCategories.FirstOrDefault(sc =>
                sc.CatalogSubCategoryId == productCategory.ParentSubCategory.CatalogSubCategoryId);
            existingProductCategory.ParentSubCategory = parentSubCategory;
            await CatalogContext.SaveChangesAsync();
        }
    }
}
