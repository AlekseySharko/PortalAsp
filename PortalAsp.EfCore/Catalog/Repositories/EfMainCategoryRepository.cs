using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortalAsp.EfCore.Helpers;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Repositories.CatalogCategories;

namespace PortalAsp.EfCore.Catalog.Repositories
{
    public class EfMainCategoryRepository : IMainCategoryRepository
    {
        private CatalogContext CatalogContext { get; }
        public EfMainCategoryRepository(CatalogContext catalogContext) => CatalogContext = catalogContext;

        public IQueryable<CatalogMainCategory> GetAllCategories(bool includeSubcategories = false,
            bool includeProductCategories = false)
        {
            if (includeSubcategories && includeProductCategories)
            {
                var result = CatalogContext.CatalogMainCategories
                    .Include(mc => mc.SubCategories)
                    .ThenInclude(sc => sc.ProductCategories);
                CatalogRefBreaker.BreakSubcategoryInfiniteReferenceCircle(result, true);
                return result;
            }

            if (includeSubcategories)
            {
                var result = CatalogContext.CatalogMainCategories
                    .Include(mc => mc.SubCategories);
                CatalogRefBreaker.BreakSubcategoryInfiniteReferenceCircle(result);
                return result;
            }

            return CatalogContext.CatalogMainCategories.AsNoTracking();
        }

        public async Task AddMainCategoryAsync(CatalogMainCategory catalogMainCategory)
        {
            await CatalogContext.CatalogMainCategories.AddAsync(catalogMainCategory);
            await CatalogContext.SaveChangesAsync();
        }

        public async Task UpdateMainCategoryAsync(CatalogMainCategory catalogMainCategory)
        {
            CatalogContext.CatalogMainCategories.Update(catalogMainCategory);
            await CatalogContext.SaveChangesAsync();
        }

        public async Task DeleteMainCategoryAsync(CatalogMainCategory catalogMainCategory)
        {
            CatalogContext.CatalogMainCategories.Remove(catalogMainCategory);
            await CatalogContext.SaveChangesAsync();
        }
    }
}
