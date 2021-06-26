using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult GetMainCategories([FromQuery] bool includeSubcategories = false, bool includeProductCategories = false)
        {
            if (includeSubcategories && includeProductCategories)
            {
                var result = CatalogContext.CatalogMainCategories.
                    Include(mc => mc.SubCategories)
                    .ThenInclude(sc => sc.ProductCategories);
                BreakSubcategoryInfiniteReferenceCircle(result);
                return Ok(result);
            }
            if (includeSubcategories)
            {
                var result = CatalogContext.CatalogMainCategories.Include(mc => mc.SubCategories);
                BreakSubcategoryInfiniteReferenceCircle(result);
                return Ok(result);
            }
            return Ok(CatalogContext.CatalogMainCategories);
        }

        [HttpPost]
        public IActionResult PostMainCategory([FromBody] CatalogMainCategory mainCategory)
        {
            ValidationResult validationResult =
                CatalogMainCategoryValidator.ValidateOnAdd(mainCategory, CatalogContext.CatalogMainCategories);
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.CatalogMainCategories.Add(mainCategory);
            CatalogContext.SaveChanges();
            return Ok();
        }

        private void BreakSubcategoryInfiniteReferenceCircle(IEnumerable<CatalogMainCategory> mainCategories)
        {
            foreach (CatalogMainCategory catalogMainCategory in mainCategories)
            {
                foreach (var catalogSubCategory in catalogMainCategory.SubCategories)
                {
                    catalogSubCategory.ParentMainCategory = null;
                }
            }
        }
    }
}
