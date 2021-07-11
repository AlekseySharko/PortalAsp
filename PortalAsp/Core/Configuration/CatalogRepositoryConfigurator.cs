using Microsoft.Extensions.DependencyInjection;
using PortalAsp.EfCore.Catalog.Repositories;
using PortalModels.Catalog.Repositories.CatalogCategories;
using PortalModels.Catalog.Repositories.Products;

namespace PortalAsp.Core.Configuration
{
    public static class CatalogRepositoryConfigurator
    {
        public static void AddEfCatalogRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMainCategoryRepository, EfMainCategoryRepository>();
            services.AddScoped<ISubCategoryRepository, EfSubCategoryRepository>();
            services.AddScoped<IManufacturerRepository, EfManufacturerRepository>();
            services.AddScoped<IProductCategoryRepository, EfProductCategoryRepository>();
            services.AddScoped<IProductRepository, EfProductRepository>();
        }
    }
}
