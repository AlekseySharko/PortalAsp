using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalAsp.EfCore.Catalog;

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
    }
}
