using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortalModels.Catalog.Products;
using PortalModels.Catalog.Repositories.Products;

namespace PortalAsp.EfCore.Catalog.Repositories
{
    public class EfManufacturerRepository : IManufacturerRepository
    {
        public CatalogContext CatalogContext { get; set; }
        public EfManufacturerRepository(CatalogContext catalogContext) => CatalogContext = catalogContext;

        public IQueryable<Manufacturer> GetAllManufacturers()
        {
            return CatalogContext.Manufacturers.AsNoTracking();
        }

        public async Task AddManufacturerAsync(Manufacturer manufacturer)
        {
            CatalogContext.Manufacturers.Add(manufacturer);
            await CatalogContext.SaveChangesAsync();
        }

        public async Task UpdateManufacturerAsync(Manufacturer manufacturer)
        {
            CatalogContext.Manufacturers.Update(manufacturer);
            await CatalogContext.SaveChangesAsync();
        }

        public async Task DeleteManufacturerAsync(Manufacturer manufacturer)
        {
            CatalogContext.Manufacturers.Remove(manufacturer);
            await CatalogContext.SaveChangesAsync();
        }
    }
}
