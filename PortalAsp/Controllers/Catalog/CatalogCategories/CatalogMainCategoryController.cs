using Microsoft.AspNetCore.Mvc;
using PortalAsp.Controllers.Helpers;
using PortalAsp.Controllers.Helpers.Catalog;
using PortalAsp.EfCore.Catalog;
using PortalModels.Catalog.CatalogCategories;

namespace PortalAsp.Controllers.Catalog.CatalogCategories
{
    [Route("api/catalog/main-categories")]
    public class CatalogMainCategoryController : Controller
    {
        public CatalogContext CatalogContext { get; set; }
        public CatalogMainCategoryController(CatalogContext catalogContext) => CatalogContext = catalogContext;

        [HttpGet]
        public IActionResult GetMainCategories()
        {
            return Ok(CatalogContext.CatalogMainCategories);
        }

        [HttpPost]
        public IActionResult PostMainCategory([FromBody]CatalogMainCategory mainCategory)
        {
            ValidationResult validationResult =
                CatalogMainCategoryValidator.ValidateOnAdd(mainCategory, CatalogContext.CatalogMainCategories);
            if (validationResult.IsValid == false) return BadRequest(validationResult.Message);

            CatalogContext.CatalogMainCategories.Add(mainCategory);
            CatalogContext.SaveChanges();
            return Ok();
        }
        

    }
}
