using System.Linq;
using System.Threading.Tasks;
using PortalModels.Catalog.Products;

namespace PortalModels.Catalog.Repositories.Products
{
    public interface IProductCategoryRepository
    {
        IQueryable<ProductCategory> GetAllProductCategories();
        Task<GeneralResult> AddProductCategoryAsync(ProductCategory productCategory, long subCategoryId);
        Task UpdateProductCategoryAsync(ProductCategory productCategory);
        Task DeleteProductCategoryAsync(ProductCategory productCategory);
    }
}
