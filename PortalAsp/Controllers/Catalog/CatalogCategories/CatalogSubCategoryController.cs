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

namespace PortalAsp.Controllers.Catalog.CatalogCategories
{
    [Route("api/catalog/sub-categories")]
    public class CatalogSubCategoryController : Controller
    {
        public CatalogContext CatalogContext { get; set; }
        public CatalogSubCategoryController(CatalogContext catalogContext) => CatalogContext = catalogContext;

        [HttpGet]
        public IActionResult GetSubCategories(long mainCategoryId)
        {
            var result = CatalogContext.CatalogSubCategories
                .Where(sc => sc.ParentMainCategory.CatalogMainCategoryId == mainCategoryId)
                .Include(sc => sc.ProductCategories);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostSubcategory([FromBody] CatalogSubCategory subCategory, [FromQuery] long mainCategoryId)
        {
            CatalogMainCategory mainCategory = await CatalogContext.CatalogMainCategories
                .Include(mc => mc.SubCategories)
                .FirstOrDefaultAsync(mc => mc.CatalogMainCategoryId == mainCategoryId);
            if (mainCategory is null)
                return BadRequest("No such main category or no main category id is provided");

            ValidationResult validationResult =
                CatalogSubCategoryValidator.ValidateOnAdd(subCategory, CatalogContext.CatalogSubCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            mainCategory.SubCategories ??= new List<CatalogSubCategory>();
            mainCategory.SubCategories.Add(subCategory);
            await CatalogContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutSubcategory([FromBody] CatalogSubCategory subCategory)
        {
            ValidationResult validationResult =
                CatalogSubCategoryValidator.ValidateOnEdit(subCategory,
                    CatalogContext.CatalogSubCategories.AsNoTracking(),
                    CatalogContext.CatalogMainCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await EditSubcategory(subCategory);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubcategory([FromRoute] long id)
        {
            CatalogSubCategory subCategory = new CatalogSubCategory {CatalogSubCategoryId = id};

            ValidationResult validationResult =
                CatalogSubCategoryValidator.ValidateOnDelete(subCategory, CatalogContext.CatalogSubCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.CatalogSubCategories.Remove(subCategory);
            await CatalogContext.SaveChangesAsync();
            return Ok();
        }

        private async Task EditSubcategory(CatalogSubCategory subCategory)
        { 
            CatalogSubCategory existingCategory = CatalogContext.CatalogSubCategories.FirstOrDefault(sc =>
                sc.CatalogSubCategoryId == subCategory.CatalogSubCategoryId);
            if(existingCategory == null) throw new Exception("No such subcategory");

            existingCategory.Name = subCategory.Name;
            CatalogMainCategory parentMainCategory = CatalogContext.CatalogMainCategories.FirstOrDefault(mc =>
                mc.CatalogMainCategoryId == subCategory.ParentMainCategory.CatalogMainCategoryId);
            existingCategory.ParentMainCategory = parentMainCategory;
            await CatalogContext.SaveChangesAsync();
        }
    }
}
