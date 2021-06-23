using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Products;
using System.Collections.Generic;

namespace PortalAsp.EfCore.Catalog
{
    public static class CatalogContextSeeder
    {
        public static void Seed(CatalogContext catalogContext)
        {
            catalogContext.CatalogMainCategories.Add(
                new CatalogMainCategory
                {
                    Name = "Electronics",
                    ImageAddress = "https://svgshare.com/i/YDF.svg",
                    SubCategories = new List<CatalogSubCategory>
                    {
                        new CatalogSubCategory
                        {
                            Name = "Mobile phones and accessories",
                            ProductCategories = new List<ProductCategory>
                            {
                                new ProductCategory{ Name = "Mobile phones" },
                                new ProductCategory{ Name = "Headphones" },
                                new ProductCategory{ Name = "Bluetooth headsets" },
                                new ProductCategory{ Name = "Phone cases" },
                                new ProductCategory{ Name = "Cables and adapters" },
                                new ProductCategory{ Name = "Charging device" },
                                new ProductCategory{ Name = "Portable chargers" },
                                new ProductCategory{ Name = "Fitness bracelets" },
                                new ProductCategory{ Name = "Batteries" }
                            }
                        },
                        new CatalogSubCategory
                        {
                            Name = "Television and video",
                            ProductCategories = new List<ProductCategory>
                            {
                                new ProductCategory{ Name = "TV sets" },
                                new ProductCategory{ Name = "Projectors" },
                                new ProductCategory{ Name = "Blu-ray players" },
                                new ProductCategory{ Name = "TV accessories" },
                                new ProductCategory{ Name = "Projection screens" },
                            }
                        },
                        new CatalogSubCategory
                        {
                            Name = "Tablets and e-books"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Audio engineering"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Hi-Fi audio"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Photo and video equipment"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Video games"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Gadgets"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Smart home and video surveillance"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Electric transport"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Telephony and communication"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Musical equipment"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Optical instruments"
                        }
                    }
                });
            catalogContext.SaveChanges();
            catalogContext.CatalogMainCategories.Add(
                new CatalogMainCategory
                {
                    Name = "Computers",
                    ImageAddress = "https://svgshare.com/i/YDX.svg",
                    SubCategories = new List<CatalogSubCategory>
                    {
                        new CatalogSubCategory
                        {
                            Name = "Laptops, computers, monitors"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Computer components"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Devices for printing and design"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Input devices"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Network hardware"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Games and software"
                        },
                        new CatalogSubCategory
                        {
                            Name = "Accessories for computers and laptops"
                        }
                    }
                });

            catalogContext.CatalogMainCategories.Add(
                new CatalogMainCategory
                {
                    Name = "Appliances",
                    ImageAddress = "https://svgshare.com/i/YB2.svg",
                });

            catalogContext.CatalogMainCategories.Add(
                new CatalogMainCategory
                {
                    Name = "Construction and Repair",
                    ImageAddress = "https://svgshare.com/i/YDM.svg"
                });

            catalogContext.CatalogMainCategories.Add(
                new CatalogMainCategory
                {
                    Name = "House and Garden",
                    ImageAddress = "https://svgshare.com/i/YCe.svg"
                });

            catalogContext.CatalogMainCategories.Add(
                new CatalogMainCategory
                {
                    Name = "Auto and Moto",
                    ImageAddress = "https://svgshare.com/i/YCf.svg"
                });
            catalogContext.CatalogMainCategories.Add(
                new CatalogMainCategory
                {
                    Name = "Beauty and Sport",
                    ImageAddress = "https://svgshare.com/i/YE0.svg"
                });

            catalogContext.CatalogMainCategories.Add(
                new CatalogMainCategory
                {
                    Name = "For Babies",
                    ImageAddress = "https://svgshare.com/i/YBy.svg"
                });

            catalogContext.CatalogMainCategories.Add(
                new CatalogMainCategory
                {
                    Name = "Work and Office",
                    ImageAddress = "https://svgshare.com/i/YE1.svg"
                });
            catalogContext.CatalogMainCategories.Add(
                new CatalogMainCategory
                {
                    Name = "Food",
                    ImageAddress = "https://svgshare.com/i/YCr.svg"
                });
            catalogContext.SaveChanges();
        }
    }
}
