using System;
using System.Collections.Generic;
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
    [Route("api/catalog/products")]
    public class ProductController : Controller
    {
        public CatalogContext CatalogContext { get; set; }
        public ProductController(CatalogContext catalogContext) => CatalogContext = catalogContext;

        [HttpGet]
        public IActionResult GetProducts(long productCategoryId)
        {
            IEnumerable<Product> requestedProducts = CatalogContext.ProductCategories
                .First(pc => pc.ProductCategoryId == productCategoryId).Products;
            return Ok(requestedProducts);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product, [FromQuery] long productCategoryId)
        {
            ProductCategory productCategory = await CatalogContext.ProductCategories
                .Include(pc => pc.Products)
                .FirstOrDefaultAsync(pc => pc.ProductCategoryId == productCategoryId);
            if (productCategory is null)
                return BadRequest("No such product category or no product category id is provided");

            ValidationResult validationResult =
                ProductValidator.ValidateOnAdd(product, CatalogContext.Products, CatalogContext.Manufacturers);
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            productCategory.Products ??= new List<Product>();
            productCategory.Products.Add(product);
            await CatalogContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutProduct([FromBody] Product product)
        {
            ValidationResult validationResult =
                ProductValidator.ValidateOnEdit(product, CatalogContext.Products.AsNoTracking(),
                    CatalogContext.Manufacturers.AsNoTracking(), CatalogContext.CatalogSubCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await EditProduct(product);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] long id)
        {
            Product productToDelete = new Product {ProductId = id};
            ValidationResult validationResult =
                ProductValidator.ValidateOnDelete(productToDelete, CatalogContext.Products.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.Products.Remove(productToDelete);
            await CatalogContext.SaveChangesAsync();
            return Ok();
        }

        private async Task EditProduct(Product product)
        {
            Product existingProduct = CatalogContext.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existingProduct == null) throw new Exception("No such product");

            existingProduct.Name = product.Name;
            existingProduct.ShortDescription = product.ShortDescription;
            existingProduct.Price = product.Price;
            existingProduct.Images = product.Images;
            existingProduct.Manufacturer =
                CatalogContext.Manufacturers.FirstOrDefault(
                    m => m.ManufacturerId == product.Manufacturer.ManufacturerId);
            existingProduct.Category =
                CatalogContext.ProductCategories.FirstOrDefault(pc =>
                    pc.ProductCategoryId == product.Category.ProductCategoryId);

            await CatalogContext.SaveChangesAsync();
        }
    }
}
