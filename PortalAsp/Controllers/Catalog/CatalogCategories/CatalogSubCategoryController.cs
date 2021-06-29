using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalAsp.Controllers.Helpers;
using PortalAsp.Controllers.Helpers.Catalog;
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
        public IActionResult PostSubcategory([FromBody] CatalogSubCategory subCategory, [FromQuery] long mainCategoryId)
        {
            CatalogMainCategory mainCategory = CatalogContext.CatalogMainCategories
                .Include(mc => mc.SubCategories)
                .FirstOrDefault(mc => mc.CatalogMainCategoryId == mainCategoryId);
            if (mainCategory is null)
                return BadRequest("No such main category or no main category id is provided");

            ValidationResult validationResult =
                CatalogSubCategoryValidator.ValidateOnAdd(subCategory, CatalogContext.CatalogSubCategories);
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            mainCategory.SubCategories ??= new List<CatalogSubCategory>();
            mainCategory.SubCategories.Add(subCategory);
            CatalogContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult PutSubcategory([FromBody] CatalogSubCategory subCategory)
        {
            ValidationResult validationResult =
                CatalogSubCategoryValidator.ValidateOnEdit(subCategory, CatalogContext.CatalogSubCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);
            
            CatalogContext.CatalogSubCategories.Update(subCategory);
            CatalogContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteSubcategory([FromRoute] long id)
        {
            CatalogSubCategory subCategory = new CatalogSubCategory {CatalogSubCategoryId = id};

            ValidationResult validationResult =
                CatalogSubCategoryValidator.ValidateOnDelete(subCategory, CatalogContext.CatalogSubCategories.AsNoTracking());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            CatalogContext.CatalogSubCategories.Remove(subCategory);
            CatalogContext.SaveChanges();
            return Ok();
        }
    }
}
