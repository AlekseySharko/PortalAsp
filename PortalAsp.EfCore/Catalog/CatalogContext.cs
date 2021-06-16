using Microsoft.EntityFrameworkCore;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Products;

namespace PortalAsp.EfCore.Catalog
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> opts)
            :base(opts) { }

        public DbSet<CatalogMainCategory> CatalogMainCategories { get; set; }
        public DbSet<CatalogSubCategory> CatalogSubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CatalogContextHelper.DefineProductCategory(modelBuilder);
            CatalogContextHelper.DefineImage(modelBuilder);
            CatalogContextHelper.DefineCatalogSubCategory(modelBuilder);
            CatalogContextHelper.DefineCatalogMainCategory(modelBuilder);
            CatalogContextHelper.DefineManufacturer(modelBuilder);
            CatalogContextHelper.DefineProduct(modelBuilder);
        }
    }
}
