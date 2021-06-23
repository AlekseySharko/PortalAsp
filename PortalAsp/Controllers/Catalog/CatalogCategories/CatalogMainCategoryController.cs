using System;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult AddCategory([FromBody]CatalogMainCategory category)
        {
            if (!CheckCategory(category)) return BadRequest("Invalid category");
            CatalogContext.CatalogMainCategories.Add(category);
            CatalogContext.SaveChanges();
            return Ok();
        }

        private bool CheckCategory(CatalogMainCategory category)
        {
            if (string.IsNullOrWhiteSpace(category.Name)) return false;
            if (string.IsNullOrWhiteSpace(category.ImageAddress)) return false;
            return true;
        }

    }
}
