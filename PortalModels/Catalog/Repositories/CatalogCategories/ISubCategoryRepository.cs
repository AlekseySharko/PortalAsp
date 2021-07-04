using System.Linq;
using System.Threading.Tasks;
using PortalModels.Catalog.CatalogCategories;

namespace PortalModels.Catalog.Repositories.CatalogCategories
{
    public interface ISubCategoryRepository
    {
        IQueryable<CatalogSubCategory> GetAllSubCategories();
        IQueryable<CatalogSubCategory> GetSubCategoriesBy(long mainCategoryId, bool includeProductCategories);
        Task<GeneralResult> AddSubcategoryAsync(CatalogSubCategory subCategory, long mainCategoryId);
        Task UpdateSubcategoryAsync(CatalogSubCategory subCategory);
        Task DeleteSubcategoryAsync(CatalogSubCategory subCategory);
    }
}
