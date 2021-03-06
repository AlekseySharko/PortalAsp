// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PortalAsp.EfCore.Catalog;

namespace PortalAsp.EfCore.Migrations
{
    [DbContext(typeof(CatalogContext))]
    [Migration("20210620180310_ProductCategoryUnique")]
    partial class ProductCategoryUnique
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PortalModels.Catalog.CatalogCategories.CatalogMainCategory", b =>
                {
                    b.Property<long>("CatalogMainCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ImageAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("NVARCHAR(50)");

                    b.HasKey("CatalogMainCategoryId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("CatalogMainCategories");
                });

            modelBuilder.Entity("PortalModels.Catalog.CatalogCategories.CatalogSubCategory", b =>
                {
                    b.Property<long>("CatalogSubCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("NVARCHAR(50)");

                    b.Property<long?>("ParentMainCategoryCatalogMainCategoryId")
                        .HasColumnType("bigint");

                    b.HasKey("CatalogSubCategoryId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.HasIndex("ParentMainCategoryCatalogMainCategoryId");

                    b.ToTable("CatalogSubCategories");
                });

            modelBuilder.Entity("PortalModels.Catalog.Products.Manufacturer", b =>
                {
                    b.Property<long>("ManufacturerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasColumnType("NVARCHAR(50)");

                    b.Property<string>("Name")
                        .HasColumnType("NVARCHAR(50)");

                    b.HasKey("ManufacturerId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("PortalModels.Catalog.Products.Product", b =>
                {
                    b.Property<long>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CategoryProductCategoryId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ManufacturerId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(50)");

                    b.Property<long>("Popularity")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(8,2)");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(150)");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryProductCategoryId");

                    b.HasIndex("ManufacturerId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PortalModels.Catalog.Products.ProductCategory", b =>
                {
                    b.Property<long>("ProductCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("CatalogSubCategoryId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasColumnType("NVARCHAR(50)");

                    b.HasKey("ProductCategoryId");

                    b.HasIndex("CatalogSubCategoryId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("PortalModels.Catalog.Products.ProductImage", b =>
                {
                    b.Property<long>("ProductImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ProductId")
                        .HasColumnType("bigint");

                    b.HasKey("ProductImageId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImage");
                });

            modelBuilder.Entity("PortalModels.Catalog.CatalogCategories.CatalogSubCategory", b =>
                {
                    b.HasOne("PortalModels.Catalog.CatalogCategories.CatalogMainCategory", "ParentMainCategory")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentMainCategoryCatalogMainCategoryId");
                });

            modelBuilder.Entity("PortalModels.Catalog.Products.Product", b =>
                {
                    b.HasOne("PortalModels.Catalog.Products.ProductCategory", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryProductCategoryId");

                    b.HasOne("PortalModels.Catalog.Products.Manufacturer", "Manufacturer")
                        .WithMany()
                        .HasForeignKey("ManufacturerId");
                });

            modelBuilder.Entity("PortalModels.Catalog.Products.ProductCategory", b =>
                {
                    b.HasOne("PortalModels.Catalog.CatalogCategories.CatalogSubCategory", null)
                        .WithMany("ProductCategories")
                        .HasForeignKey("CatalogSubCategoryId");
                });

            modelBuilder.Entity("PortalModels.Catalog.Products.ProductImage", b =>
                {
                    b.HasOne("PortalModels.Catalog.Products.Product", null)
                        .WithMany("Images")
                        .HasForeignKey("ProductId");
                });
#pragma warning restore 612, 618
        }
    }
}
