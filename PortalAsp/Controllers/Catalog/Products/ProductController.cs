using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalModels;
using PortalModels.Catalog.Products;
using PortalModels.Catalog.Repositories.Products;
using PortalModels.Validators;
using PortalModels.Validators.Catalog;

namespace PortalAsp.Controllers.Catalog.Products
{
    [Route("api/catalog/products")]
    public class ProductController : Controller
    {
        private IProductRepository ProductRepository { get; }
        private IProductCategoryRepository ProductCategoryRepository { get; }
        private IManufacturerRepository ManufacturerRepository { get; }
        public ProductController(IProductRepository productRepository, IManufacturerRepository manufacturerRepository, IProductCategoryRepository productCategoryRepository)
        {
            ProductRepository = productRepository;
            ProductCategoryRepository = productCategoryRepository;
            ManufacturerRepository = manufacturerRepository;
        }

        [HttpGet]
        public IActionResult GetProducts(long productCategoryId)
        {
            return Ok(ProductRepository.GetProductsBy(productCategoryId));
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product, [FromQuery] long productCategoryId)
        {
            ValidationResult validationResult =
                ProductValidator.ValidateOnAdd(product, ProductRepository.GetAllProducts(), ManufacturerRepository.GetAllManufacturers());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            GeneralResult result = await ProductRepository.AddProductAsync(product, productCategoryId);
            if (!result.Success) 
                return BadRequest(result.Message);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutProduct([FromBody] Product product)
        {
            ValidationResult validationResult =
                ProductValidator.ValidateOnEdit(product, ProductRepository.GetAllProducts(),
                    ManufacturerRepository.GetAllManufacturers(), ProductCategoryRepository.GetAllProductCategories());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await ProductRepository.UpdateProductAsync(product);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] long id)
        {
            Product productToDelete = new Product {ProductId = id};
            ValidationResult validationResult =
                ProductValidator.ValidateOnDelete(productToDelete, ProductRepository.GetAllProducts());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await ProductRepository.DeleteProductAsync(productToDelete);
            return Ok();
        }
    }
}
