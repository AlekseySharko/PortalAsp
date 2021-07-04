using System.Linq;
using System.Threading.Tasks;
using PortalModels.Catalog.Products;

namespace PortalModels.Catalog.Repositories.Products
{
    public interface IManufacturerRepository
    {
        IQueryable<Manufacturer> GetAllManufacturers();
        Task AddManufacturerAsync(Manufacturer manufacturer);
        Task UpdateManufacturerAsync(Manufacturer manufacturer);
        Task DeleteManufacturerAsync(Manufacturer manufacturer);
    }
}
