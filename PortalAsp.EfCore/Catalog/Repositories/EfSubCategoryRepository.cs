using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortalModels;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Repositories.CatalogCategories;

namespace PortalAsp.EfCore.Catalog.Repositories
{
    public class EfSubCategoryRepository : ISubCategoryRepository
    {
        private CatalogContext CatalogContext { get; }
        public EfSubCategoryRepository(CatalogContext catalogContext) => CatalogContext = catalogContext;

        public IQueryable<CatalogSubCategory> GetAllSubCategories()
        {
            return CatalogContext.CatalogSubCategories.AsNoTracking();
        }

        public IQueryable<CatalogSubCategory> GetSubCategoriesBy(long mainCategoryId,
            bool includeProductCategories = false)
        {
            if (includeProductCategories)
            {
                return CatalogContext.CatalogSubCategories
                    .Where(sc => sc.ParentMainCategory.CatalogMainCategoryId == mainCategoryId)
                    .Include(sc => sc.ProductCategories);
            }

            return CatalogContext.CatalogSubCategories
                .AsNoTracking()
                .Where(sc => sc.ParentMainCategory.CatalogMainCategoryId == mainCategoryId);
        }

        public async Task<GeneralResult> AddSubcategoryAsync(CatalogSubCategory subCategory, long mainCategoryId)
        {
            CatalogMainCategory mainCategory = await CatalogContext.CatalogMainCategories
                .Include(mc => mc.SubCategories)
                .FirstOrDefaultAsync(mc => mc.CatalogMainCategoryId == mainCategoryId);
            if (mainCategory is null)
            {
                return new GeneralResult
                {
                    Success = false, 
                    Message = "No such main category or no main category id is provided"
                };
            }
            mainCategory.SubCategories ??= new List<CatalogSubCategory>();
            mainCategory.SubCategories.Add(subCategory);
            await CatalogContext.SaveChangesAsync();
            return new GeneralResult {Success = true};
        }

        public async Task UpdateSubcategoryAsync(CatalogSubCategory subCategory)
        {
            CatalogSubCategory existingCategory = CatalogContext.CatalogSubCategories.FirstOrDefault(sc =>
                sc.CatalogSubCategoryId == subCategory.CatalogSubCategoryId);
            if (existingCategory == null) throw new Exception("No such subcategory");

            existingCategory.Name = subCategory.Name;
            CatalogMainCategory parentMainCategory = CatalogContext.CatalogMainCategories.FirstOrDefault(mc =>
                mc.CatalogMainCategoryId == subCategory.ParentMainCategory.CatalogMainCategoryId);
            existingCategory.ParentMainCategory = parentMainCategory;
            await CatalogContext.SaveChangesAsync();
        }

        public async Task DeleteSubcategoryAsync(CatalogSubCategory subCategory)
        {
            CatalogContext.CatalogSubCategories.Remove(subCategory);
            await CatalogContext.SaveChangesAsync();
        }
    }
}
