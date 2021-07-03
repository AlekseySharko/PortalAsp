using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PortalAsp.Controllers.Catalog.Products;
using PortalAsp.EfCore.Catalog;
using PortalModels.Catalog.Products;
using Xunit;

namespace PortalAsp.Tests.Controllers.Catalog.Products
{
    public class ProductControllerTests
    {
        private static readonly Manufacturer TestManufacturer = new Manufacturer
        {
            ManufacturerId = 1,
            Name = "Initial Manufacturer",
            Country = "USA"
        };
        //TODO - edit validation tests
        [Theory]
        [ClassData(typeof(BadRequestTestData))]
        public async Task PostProduct_DropsBadRequestOnInvalidProductOrProductCategoryId(
            Product product, int productCategoryId, bool isOk, string errorMessage = "")
        {
            //Arrange
            Mock<CatalogContext> mock = new Mock<CatalogContext>(new DbContextOptions<CatalogContext>());
            var mockObj = mock.Object;

            DbSet<ProductCategory> productCategorySet = TestHelper.GetQueryableMockDbSet(new ProductCategory()
            {
                ProductCategoryId = 444,
                Name = "Initial Product Category"
            });
            mockObj.ProductCategories = productCategorySet;

            DbSet<Product> productSet = TestHelper.GetQueryableMockDbSet(new Product
            {
                ProductId = 4,
                Category = new ProductCategory(),
                Images = new List<ProductImage> { new ProductImage() },
                Manufacturer = new Manufacturer(),
                Name = "Initial Product",
                Popularity = 0,
                Price = 1.99M,
                ShortDescription = "Short description"
            });
            mockObj.Products = productSet;

            DbSet<Manufacturer> dbSet = TestHelper.GetQueryableMockDbSet(TestManufacturer);
            mockObj.Manufacturers = dbSet;

            ProductController controller = new ProductController(mockObj);

            //Act
            IActionResult postResult = await controller.PostProduct(product, productCategoryId);
            OkResult okResult = postResult as OkResult;
            BadRequestObjectResult badRequestResult = postResult as BadRequestObjectResult;

            //Assert
            if (isOk)
            {
                Assert.NotNull(okResult);
            }
            else
            {
                Assert.NotNull(badRequestResult);
                Assert.Equal(badRequestResult.Value.ToString(), errorMessage);
            }
        }

        public class BadRequestTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Control
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = TestManufacturer,
                        Name = "Test product",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    444,
                    true
                };

                //ProductCategoryId
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = TestManufacturer,
                        Name = "Test product",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    333,
                    false,
                    "No such product category or no product category id is provided"
                };
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = TestManufacturer,
                        Name = "Test product",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    0,
                    false,
                    "No such product category or no product category id is provided"
                };

                //Null object
                yield return new object[]
                {
                    null,
                    444,
                    false,
                    "Product is null. "
                };

                //Id
                yield return new object[]
                {
                    new Product
                    {
                        ProductId = 123,
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = TestManufacturer,
                        Name = "Test product",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    444,
                    false,
                    "ProductCategoryId should be 0 or undefined. "
                };

                //Name
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = TestManufacturer,
                        Name = "   ",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    444,
                    false,
                    "Name is invalid. "
                };
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = TestManufacturer,
                        Name = "Initial product",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    444,
                    false,
                    "There's a product with the same name. "
                };
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = TestManufacturer,
                        Name = "Initial roduct",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    444,
                    true
                };

                //ShortDescription
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = TestManufacturer,
                        Name = "Test product",
                        Price = 1.99M,
                        ShortDescription = "   "
                    },
                    444,
                    false,
                    "ShortDescription is invalid. "
                };

                //Popularity
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = TestManufacturer,
                        Name = "Test product",
                        Price = 1.99M,
                        Popularity = -2,
                        ShortDescription = "Test short description"
                    },
                    444,
                    false,
                    "Popularity must be 0. "
                };

                //Images
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>( ),
                        Manufacturer = TestManufacturer,
                        Name = "Test product",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    444,
                    false,
                    "A product must have at least one image"
                };
                yield return new object[]
                {
                    new Product
                    {
                        Images = null,
                        Manufacturer = TestManufacturer,
                        Name = "Test product",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    444,
                    false,
                    "A product must have at least one image"
                };

                //Manufacturer
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = new Manufacturer(),
                        Name = "Test product",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    444,
                    false,
                    "Such manufacturer doesn't exist"
                };
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = new Manufacturer {ManufacturerId = 123},
                        Name = "Test product",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    444,
                    false,
                    "Such manufacturer doesn't exist"
                };
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = new Manufacturer {ManufacturerId = 1},
                        Name = "Test product",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    444,
                    true
                };

                //Category
                yield return new object[]
                {
                    new Product
                    {
                        Category = new ProductCategory(),
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = TestManufacturer,
                        Name = "Test product",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    444,
                    false,
                    "Product category must be null"
                };

                //Delete it
                yield return new object[]
                {
                    new Product
                    {
                        Images = new List<ProductImage>{ new ProductImage() },
                        Manufacturer = TestManufacturer,
                        Name = "Test product",
                        Price = 1.99M,
                        ShortDescription = "Test short description"
                    },
                    444,
                    true
                };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}