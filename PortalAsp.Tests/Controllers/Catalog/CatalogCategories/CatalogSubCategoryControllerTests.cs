using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PortalAsp.Controllers.Catalog.CatalogCategories;
using PortalAsp.EfCore.Catalog;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Products;
using Xunit;

namespace PortalAsp.Tests.Controllers.Catalog.CatalogCategories
{
    public class CatalogSubCategoryControllerTests
    {
        [Theory]
        [ClassData(typeof(BadPostRequestTestData))]
        public async Task PostSubCategory_DropsBadRequestOnInvalidSubCategoryOrMainCategoryId(CatalogSubCategory subCategory, int mainCategoryId, string resultType)
        {
            //Arrange
            Mock<CatalogContext> mock = new Mock<CatalogContext>(new DbContextOptions<CatalogContext>());
            var mockObj = mock.Object;

            DbSet<CatalogMainCategory> mainCategorySet = TestHelper.GetQueryableMockDbSet(new CatalogMainCategory
            {
                CatalogMainCategoryId = 1,
                Name = "Initial Main Category"
            });
            mockObj.CatalogMainCategories = mainCategorySet;

            DbSet<CatalogSubCategory> subCategoriesSet = TestHelper.GetQueryableMockDbSet(new CatalogSubCategory
            {
                CatalogSubCategoryId = 2,
                Name = "Initial Sub Category"
            });
            mockObj.CatalogSubCategories = subCategoriesSet;

            CatalogSubCategoryController controller = new CatalogSubCategoryController(mockObj);

            //Act
            IActionResult postResult = await controller.PostSubcategory(subCategory, mainCategoryId);

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
                    new CatalogSubCategory
                    {
                        CatalogSubCategoryId = 0,
                        Name = "Name"
                    },
                    1,
                    "OkResult"
                };
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "Name"
                    },
                    1,
                    "OkResult"
                };

                //Empty object
                yield return new object[]
                {
                    new CatalogSubCategory(),
                    1,
                    "BadRequestObjectResult"
                };

                //MainCategoryId 
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "Name"
                    },
                    555,
                    "BadRequestObjectResult"
                };

                //Id 
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        CatalogSubCategoryId = 1,
                        Name = "Name"
                    },
                    1,
                    "BadRequestObjectResult"
                };

                //Name
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "  "
                    },
                    1,
                    "BadRequestObjectResult"
                };
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "Initial sub category"
                    },
                    1,
                    "BadRequestObjectResult"
                };
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "Initial ub category"
                    },
                    1,
                    "OkResult"
                };

                //ProductCategories
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "Name",
                        ProductCategories = new List<ProductCategory>()
                    },
                    1,
                    "OkResult"
                };
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "Name",
                        ProductCategories = new List<ProductCategory>{new ProductCategory()}
                    },
                    1,
                    "BadRequestObjectResult"
                };

                //ParentMainCategory
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "Name",
                        ParentMainCategory = new CatalogMainCategory()
                    },
                    1,
                    "BadRequestObjectResult"
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(BadPutRequestTestData))]
        public async Task PutSubCategory_DropsBadRequestOnInvalidSubCategoryOrMainCategoryId(CatalogSubCategory subCategory, string resultType)
        {
            //Arrange
            Mock<CatalogContext> mock = new Mock<CatalogContext>(new DbContextOptions<CatalogContext>());
            var mockObj = mock.Object;

            DbSet<CatalogSubCategory> subCategoriesSet = TestHelper.GetQueryableMockDbSet(new CatalogSubCategory()
            {
                CatalogSubCategoryId = 32423414,
                Name = "Initial Sub Category"
            }, new CatalogSubCategory()
            {
                CatalogSubCategoryId = 1412,
                Name = "Second Sub Category"
            });
            mockObj.CatalogSubCategories = subCategoriesSet;
            DbSet<CatalogMainCategory> dbSet = TestHelper.GetQueryableMockDbSet(new CatalogMainCategory
            {
                CatalogMainCategoryId = 2134,
                Name = "Initial Main Category",
                ImageAddress = "httpfsdaf.png"
            });
            mockObj.CatalogMainCategories = dbSet;

            CatalogSubCategoryController controller = new CatalogSubCategoryController(mockObj);

            //Act
            IActionResult postResult = await controller.PutSubcategory(subCategory);

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
                    new CatalogSubCategory
                    {
                        CatalogSubCategoryId = 32423414,
                        Name = "Name",
                        ParentMainCategory = new CatalogMainCategory {CatalogMainCategoryId = 2134}
                    },
                    "OkResult"
                };

                //Empty object
                yield return new object[]
                {
                    new CatalogSubCategory(),
                    "BadRequestObjectResult"
                };

                //Id 
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        CatalogSubCategoryId = 5321512,
                        Name = "Name",
                        ParentMainCategory = new CatalogMainCategory {CatalogMainCategoryId = 2134}
                    },
                    "BadRequestObjectResult"
                };

                //Name
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        CatalogSubCategoryId = 32423414,
                        Name = "  ",
                        ParentMainCategory = new CatalogMainCategory {CatalogMainCategoryId = 2134}
                    },
                    "BadRequestObjectResult"
                };

                //Name case insensitive check
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        CatalogSubCategoryId = 32423414,
                        Name = "Initial Sub Category",
                        ParentMainCategory = new CatalogMainCategory {CatalogMainCategoryId = 2134}
                    },
                    "OkResult"
                };
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        CatalogSubCategoryId = 32423414,
                        Name = "Second Sub Category",
                        ParentMainCategory = new CatalogMainCategory {CatalogMainCategoryId = 2134}
                    },
                    "BadRequestObjectResult"
                };

                //ProductCategories
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        CatalogSubCategoryId = 32423414,
                        Name = "Name",
                        ProductCategories = new List<ProductCategory>(),
                        ParentMainCategory = new CatalogMainCategory {CatalogMainCategoryId = 2134}
                    },
                    "OkResult"
                };
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        CatalogSubCategoryId = 32423414,
                        Name = "Name",
                        ProductCategories = new List<ProductCategory>{new ProductCategory()},
                        ParentMainCategory = new CatalogMainCategory {CatalogMainCategoryId = 2134}
                    },
                    "BadRequestObjectResult"
                };

                //ParentMainCategory
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        CatalogSubCategoryId = 32423414,
                        Name = "Name",
                        ParentMainCategory = new CatalogMainCategory {CatalogMainCategoryId = 2134}
                    },
                    "OkResult"
                };
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        CatalogSubCategoryId = 32423414,
                        Name = "Name",
                        ParentMainCategory = null
                    },
                    "BadRequestObjectResult"
                };
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        CatalogSubCategoryId = 32423414,
                        Name = "Name",
                        ParentMainCategory = new CatalogMainCategory {CatalogMainCategoryId = 111}
                    },
                    "BadRequestObjectResult"
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
