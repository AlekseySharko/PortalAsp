using System.Linq;
using System.Threading.Tasks;
using PortalModels.Catalog.Products;

namespace PortalModels.Catalog.Repositories.Products
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAllProducts();
        IQueryable<Product> GetProductsBy(long productCategoryId);
        Task<GeneralResult> AddProductAsync(Product product, long productCategoryId);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
    }
}
