using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PortalAsp.Controllers.Catalog.Products;
using PortalAsp.EfCore.Catalog;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Products;
using Xunit;

namespace PortalAsp.Tests.Controllers.Catalog.Products
{
    public class ProductCategoryControllerTests
    {
        [Theory]
        [ClassData(typeof(BadPostRequestTestData))]
        public async Task PostProductCategory_DropsBadRequestOnInvalidProductCategoryOrSubCategoryId(ProductCategory productCategory, int subCategoryId, string resultType)
        {
            //Arrange
            Mock<CatalogContext> mock = new Mock<CatalogContext>(new DbContextOptions<CatalogContext>());
            var mockObj = mock.Object;

            DbSet<ProductCategory> productCategorySet = TestHelper.GetQueryableMockDbSet(new ProductCategory()
            {
                ProductCategoryId = 1,
                Name = "Initial Product Category"
            });
            mockObj.ProductCategories = productCategorySet;

            DbSet<CatalogSubCategory> subCategoriesSet = TestHelper.GetQueryableMockDbSet(new CatalogSubCategory()
            {
                CatalogSubCategoryId = 2,
                Name = "Initial Sub Category"
            });
            mockObj.CatalogSubCategories = subCategoriesSet;

            ProductCategoryController controller = new ProductCategoryController(mockObj);

            //Act
            IActionResult postResult = await controller.PostProductCategory(productCategory, subCategoryId);

            //Assert
            Assert.Equal(resultType, postResult.GetType().Name);
        }

        public class BadPostRequestTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Control
                yield return new object[]
                {
                    new ProductCategory
                    {
                        Name = "Name"
                    },
                    2,
                    "OkResult"
                };

                //Empty object
                yield return new object[] { new ProductCategory(), 2, "BadRequestObjectResult" };

                //Id
                yield return new object[]
                {
                    new ProductCategory
                    {
                        ProductCategoryId = 5,
                        Name = "Name"
                    },
                    2,
                    "BadRequestObjectResult"
                };

                //ParentSubCategory
                yield return new object[]
                {
                    new ProductCategory
                    {
                        Name = "Name",
                        ParentSubCategory = new CatalogSubCategory()
                    },
                    2,
                    "BadRequestObjectResult"
                };

                //SubCategoryId
                yield return new object[]
                {
                    new ProductCategory
                    {
                        Name = "Name"
                    },
                    0,
                    "BadRequestObjectResult"
                };

                //Name
                yield return new object[]
                {
                    new ProductCategory
                    {
                        Name = "  "
                    },
                    2,
                    "BadRequestObjectResult"
                };

                //Name case insensitive exists
                yield return new object[]
                {
                    new ProductCategory
                    {
                        Name = "Initial product category"
                    },
                    2,
                    "BadRequestObjectResult"
                };
                yield return new object[]
                {
                    new ProductCategory
                    {
                        Name = "Initial poduct category"
                    },
                    2,
                    "OkResult"
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(BadPutRequestTestData))]
        public async Task PutProductCategory_DropsBadRequestOnInvalidProductCategoryOrSubCategoryId(ProductCategory productCategory, string resultType)
        {
            //Arrange
            Mock<CatalogContext> mock = new Mock<CatalogContext>(new DbContextOptions<CatalogContext>());
            var mockObj = mock.Object;

            DbSet<ProductCategory> productCategorySet = TestHelper.GetQueryableMockDbSet(new ProductCategory()
            {
                ProductCategoryId = 142351,
                Name = "Initial Product Category"
            });
            mockObj.ProductCategories = productCategorySet;

            DbSet<CatalogSubCategory> subCategoriesSet = TestHelper.GetQueryableMockDbSet(new CatalogSubCategory
            {
                CatalogSubCategoryId = 2,
                Name = "Initial Sub Category"
            });
            mockObj.CatalogSubCategories = subCategoriesSet;

            ProductCategoryController controller = new ProductCategoryController(mockObj);

            //Act
            IActionResult postResult = await controller.PutProductCategory(productCategory);

            //Assert
            Assert.Equal(resultType, postResult.GetType().Name);
        }

        public class BadPutRequestTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Control
                yield return new object[]
                {
                    new ProductCategory
                    {
                        ProductCategoryId = 142351,
                        Name = "Name",
                        ParentSubCategory = new CatalogSubCategory{CatalogSubCategoryId = 2}
                    },
                    "OkResult"
                };

                //Empty object
                yield return new object[] { new ProductCategory(), "BadRequestObjectResult" };
                
                //Id
                yield return new object[]
                {
                    new ProductCategory
                    {
                        ProductCategoryId = 142,
                        Name = "Name",
                        ParentSubCategory = new CatalogSubCategory{CatalogSubCategoryId = 2}
                    },
                    "BadRequestObjectResult"
                };

                //ParentCategory
                yield return new object[]
                {
                    new ProductCategory
                    {
                        ProductCategoryId = 142351,
                        Name = "Name",
                        ParentSubCategory = new CatalogSubCategory{CatalogSubCategoryId = 124}
                    },
                    "BadRequestObjectResult"
                };
                yield return new object[]
                {
                    new ProductCategory
                    {
                        ProductCategoryId = 142351,
                        Name = "Name"
                    },
                    "BadRequestObjectResult"
                };

                //Name
                yield return new object[]
                {
                    new ProductCategory
                    {
                        ProductCategoryId = 142351,
                        Name = "  ",
                        ParentSubCategory = new CatalogSubCategory{CatalogSubCategoryId = 2}
                    },
                    "BadRequestObjectResult"
                };

                //Name case insensitive exists
                yield return new object[]
                {
                    new ProductCategory
                    {
                        ProductCategoryId = 142351,
                        Name = "Initial product category",
                        ParentSubCategory = new CatalogSubCategory{CatalogSubCategoryId = 2}
                    },
                    "OkResult"
                };
                yield return new object[]
                {
                    new ProductCategory
                    {
                        ProductCategoryId = 142351,
                        Name = "Initial poduct category",
                        ParentSubCategory = new CatalogSubCategory{CatalogSubCategoryId = 2}
                    },
                    "OkResult"
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
