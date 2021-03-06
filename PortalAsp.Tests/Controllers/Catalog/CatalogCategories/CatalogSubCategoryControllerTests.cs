using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PortalAsp.Controllers.Catalog.CatalogCategories;
using PortalAsp.EfCore.Catalog;
using PortalAsp.EfCore.Catalog.Repositories;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Products;
using Xunit;

namespace PortalAsp.Tests.Controllers.Catalog.CatalogCategories
{
    public class CatalogSubCategoryControllerTests
    {
        private static CatalogSubCategoryController GetController()
        {
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

            return new CatalogSubCategoryController(new EfSubCategoryRepository(mockObj), new EfMainCategoryRepository(mockObj));
        }

        [Theory]
        [ClassData(typeof(BadPostRequestTestData))]
        public async Task PostSubCategory_Validation(CatalogSubCategory subCategory, int mainCategoryId, string resultType)
        {
            //Arrange
            CatalogSubCategoryController controller = GetController();

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
                    2134,
                    "OkResult"
                };
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "Name"
                    },
                    2134,
                    "OkResult"
                };

                //Empty object
                yield return new object[]
                {
                    new CatalogSubCategory(),
                    2134,
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
                    2134,
                    "BadRequestObjectResult"
                };

                //Name
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "  "
                    },
                    2134,
                    "BadRequestObjectResult"
                };
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "Initial sub category"
                    },
                    2134,
                    "BadRequestObjectResult"
                };
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "Initial ub category"
                    },
                    2134,
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
                    2134,
                    "OkResult"
                };
                yield return new object[]
                {
                    new CatalogSubCategory
                    {
                        Name = "Name",
                        ProductCategories = new List<ProductCategory>{new ProductCategory()}
                    },
                    2134,
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
                    2134,
                    "BadRequestObjectResult"
                };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(BadPutRequestTestData))]
        public async Task PutSubCategory_Validation(CatalogSubCategory subCategory, string resultType)
        {
            //Arrange
            CatalogSubCategoryController controller = GetController();

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
