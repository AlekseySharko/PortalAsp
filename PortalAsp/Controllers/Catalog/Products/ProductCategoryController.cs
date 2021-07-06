using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalModels;
using PortalModels.Catalog.Products;
using PortalModels.Catalog.Repositories.CatalogCategories;
using PortalModels.Catalog.Repositories.Products;
using PortalModels.Validators;
using PortalModels.Validators.Catalog;

namespace PortalAsp.Controllers.Catalog.Products
{
    [Route("api/catalog/product-categories")]
    public class ProductCategoryController : Controller
    {
        public IProductCategoryRepository ProductCategoryRepository { get; set; }
        public ISubCategoryRepository SubCategoryRepository { get; set; }

        public ProductCategoryController(IProductCategoryRepository productCategoryRepository,
            ISubCategoryRepository subCategoryRepository)
        {
            ProductCategoryRepository = productCategoryRepository;
            SubCategoryRepository = subCategoryRepository;
        }

        [HttpGet]
        public IActionResult GetProductCategories()
        {
            return Ok(ProductCategoryRepository.GetAllProductCategories());
        }

        [HttpPost]
        public async Task<IActionResult> PostProductCategory([FromBody] ProductCategory productCategory,
            [FromQuery] long subCategoryId)
        {
            ValidationResult validationResult =
                ProductCategoryValidator.ValidateOnAdd(productCategory, ProductCategoryRepository.GetAllProductCategories());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            GeneralResult result = await ProductCategoryRepository.AddProductCategoryAsync(productCategory, subCategoryId);
            if (!result.Success) return BadRequest(result.Message);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutProductCategory([FromBody] ProductCategory productCategory)
        {
            ValidationResult validationResult =
                ProductCategoryValidator.ValidateOnEdit(productCategory,
                    ProductCategoryRepository.GetAllProductCategories(),
                    SubCategoryRepository.GetAllSubCategories());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await ProductCategoryRepository.UpdateProductCategoryAsync(productCategory);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCategory([FromRoute] long id)
        {
            ProductCategory productCategory = new ProductCategory {ProductCategoryId = id};

            ValidationResult validationResult =
                ProductCategoryValidator.ValidateOnDelete(productCategory, ProductCategoryRepository.GetAllProductCategories());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await ProductCategoryRepository.DeleteProductCategoryAsync(productCategory);
            return Ok();
        }
    }
}
