using Microsoft.EntityFrameworkCore;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Products;

namespace PortalAsp.EfCore.Catalog
{
    class CatalogContextHelper
    {
        public static void DefineProduct(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p=> p.Price)
                .HasColumnType("decimal(8,2)")
                .IsRequired();
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .HasColumnType("NVARCHAR(50)")
                .IsRequired();
            modelBuilder.Entity<Product>()
                .Property(p => p.ShortDescription)
                .HasColumnType("NVARCHAR(150)")
                .IsRequired();
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();
        }

        public static void DefineCatalogMainCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogMainCategory>()
                .Property(c => c.Name)
                .HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<CatalogMainCategory>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }

        public static void DefineCatalogSubCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CatalogSubCategory>()
                .Property(sc => sc.Name)
                .HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<CatalogSubCategory>()
                .HasIndex(sc => sc.Name)
                .IsUnique();
        }

        public static void DefineManufacturer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>()
                .Property(m => m.Name)
                .HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<Manufacturer>()
                .HasIndex(m => m.Name)
                .IsUnique();
            modelBuilder.Entity<Manufacturer>()
                .Property(m => m.Country)
                .HasColumnType("NVARCHAR(50)");
        }

        public static void DefineProductCategory(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .Property(pc => pc.Name)
                .HasColumnType("NVARCHAR(50)");
            modelBuilder.Entity<ProductCategory>()
                .HasIndex(pc => pc.Name)
                .IsUnique();
        }

        public static void DefineImage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductImage>()
                .Property(m => m.Address)
                .IsRequired();
        }
    }
}
