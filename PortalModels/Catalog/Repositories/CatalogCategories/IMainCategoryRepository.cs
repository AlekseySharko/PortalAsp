using System.Linq;
using System.Threading.Tasks;
using PortalModels.Catalog.CatalogCategories;

namespace PortalModels.Catalog.Repositories.CatalogCategories
{
    public interface IMainCategoryRepository
    {
        IQueryable<CatalogMainCategory> GetAllCategories(bool includeSubcategories = false,
            bool includeProductCategories = false);
        Task AddMainCategoryAsync(CatalogMainCategory catalogMainCategory);
        Task UpdateMainCategoryAsync(CatalogMainCategory catalogMainCategory);
        Task DeleteMainCategoryAsync(CatalogMainCategory catalogMainCategory);
    }
}
