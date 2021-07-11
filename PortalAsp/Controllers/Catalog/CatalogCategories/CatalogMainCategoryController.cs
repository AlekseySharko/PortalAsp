using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalAsp.Core.Filters;
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
        private IMainCategoryRepository MainCategoryRepository { get; }

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
