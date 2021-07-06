using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAsp.Filters;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Repositories.CatalogCategories;
using PortalModels.Validators;
using PortalModels.Validators.Catalog;

namespace PortalAsp.Controllers.Catalog.CatalogCategories
{
    [RoleAuthFilter("Catalog Moderator")]
    [Route("api/catalog/main-categories")]
    public class CatalogMainCategoryController : Controller
    {
        public IMainCategoryRepository MainCategoryRepository { get; set; }

        public CatalogMainCategoryController(IMainCategoryRepository mainCategoryRepository) =>
            MainCategoryRepository = mainCategoryRepository;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetMainCategories([FromQuery] bool includeSubcategories = false,
            [FromQuery] bool includeProductCategories = false)
        {
            return Ok(MainCategoryRepository.GetAllCategories(includeSubcategories, includeProductCategories));
        }

        [HttpPost]
        public async Task<IActionResult> PostMainCategory([FromBody] CatalogMainCategory mainCategory)
        {
            ValidationResult validationResult =
                CatalogMainCategoryValidator.ValidateOnAdd(mainCategory, MainCategoryRepository.GetAllCategories());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await MainCategoryRepository.AddMainCategoryAsync(mainCategory);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutMainCategory([FromBody] CatalogMainCategory mainCategory)
        {
            ValidationResult validationResult =
                CatalogMainCategoryValidator.ValidateOnEdit(mainCategory, MainCategoryRepository.GetAllCategories());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await MainCategoryRepository.UpdateMainCategoryAsync(mainCategory);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMainCategory([FromRoute] long id)
        {
            CatalogMainCategory mainCategory = new CatalogMainCategory {CatalogMainCategoryId = id};

            ValidationResult validationResult =
                CatalogMainCategoryValidator.ValidateOnDelete(mainCategory, MainCategoryRepository.GetAllCategories());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await MainCategoryRepository.DeleteMainCategoryAsync(mainCategory);
            return Ok();
        }
    }
}
