using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalAsp.Controllers.Validators;
using PortalAsp.Controllers.Validators.Catalog;
using PortalAsp.Controllers.Validators.Catalog.CircularReferenceBreakers;
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
                var result = CatalogContext.CatalogMainCategories
                    .Include(mc => mc.SubCategories)
                    .ThenInclude(sc => sc.ProductCategories);
                CatalogRefBreaker.BreakSubcategoryInfiniteReferenceCircle(result, true);
                return Ok(result);
            }
            if (includeSubcategories)
            {
                var result = CatalogContext.CatalogMainCategories
                    .Include(mc => mc.SubCategories);
                CatalogRefBreaker.BreakSubcategoryInfiniteReferenceCircle(result);
                return Ok(result);
            }
            return Ok(CatalogContext.CatalogMainCategories);
        }

        [HttpPost]
        public async Task<IActionResult> PostMainCategory([FromBody] CatalogMainCategory mainCategory)
        {
            ValidationResult validationResult =
                CatalogMainCategoryValidator.ValidateOnAdd(mainCategory, CatalogContext.CatalogMainCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await CatalogContext.CatalogMainCategories.AddAsync(mainCategory);
            await CatalogContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutMainCategory([FromBody] CatalogMainCategory mainCategory)
        {
            ValidationResult validationResult =
                CatalogMainCategoryValidator.ValidateOnEdit(mainCategory, CatalogContext.CatalogMainCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.CatalogMainCategories.Update(mainCategory);
            await CatalogContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMainCategory([FromRoute] long id)
        {
            CatalogMainCategory mainCategory = new CatalogMainCategory {CatalogMainCategoryId = id};

            ValidationResult validationResult =
                CatalogMainCategoryValidator.ValidateOnDelete(mainCategory, CatalogContext.CatalogMainCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.CatalogMainCategories.Remove(mainCategory);
            await CatalogContext.SaveChangesAsync();
            return Ok();
        }
    }
}
