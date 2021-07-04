using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAsp.Validators;
using PortalAsp.Validators.Catalog;
using PortalModels;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Repositories.CatalogCategories;

namespace PortalAsp.Controllers.Catalog.CatalogCategories
{
    [Route("api/catalog/sub-categories")]
    //[Authorize(Roles = "CatalogModerator")]
    public class CatalogSubCategoryController : Controller
    {
        private ISubCategoryRepository SubCategoryRepository { get; }
        private IMainCategoryRepository MainCategoryRepository { get; }

        public CatalogSubCategoryController(ISubCategoryRepository subCategoryRepository, IMainCategoryRepository mainCategoryRepository)
        {
            SubCategoryRepository = subCategoryRepository;
            MainCategoryRepository = mainCategoryRepository;
        }
        

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetSubCategories(long mainCategoryId, bool includeProductCategories = false)
        {
            return Ok(SubCategoryRepository.GetSubCategoriesBy(mainCategoryId, includeProductCategories));
        }

        [HttpPost]
        public async Task<IActionResult> PostSubcategory([FromBody] CatalogSubCategory subCategory, [FromQuery] long mainCategoryId)
        {
            ValidationResult validationResult =
                CatalogSubCategoryValidator.ValidateOnAdd(subCategory, SubCategoryRepository.GetAllSubCategories());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            GeneralResult result = await SubCategoryRepository.AddSubcategoryAsync(subCategory, mainCategoryId);
            if (!result.Success) 
                return BadRequest(result.Message);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutSubcategory([FromBody] CatalogSubCategory subCategory)
        {
            ValidationResult validationResult =
                CatalogSubCategoryValidator.ValidateOnEdit(subCategory,
                    SubCategoryRepository.GetAllSubCategories(),
                    MainCategoryRepository.GetAllCategories());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await SubCategoryRepository.UpdateSubcategoryAsync(subCategory);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubcategory([FromRoute] long id)
        {
            CatalogSubCategory subCategory = new CatalogSubCategory {CatalogSubCategoryId = id};

            ValidationResult validationResult =
                CatalogSubCategoryValidator.ValidateOnDelete(subCategory, SubCategoryRepository.GetAllSubCategories());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await SubCategoryRepository.DeleteSubcategoryAsync(subCategory);
            return Ok();
        }
    }
}
