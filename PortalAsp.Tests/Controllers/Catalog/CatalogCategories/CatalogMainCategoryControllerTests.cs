using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PortalAsp.Controllers.Catalog.CatalogCategories;
using PortalAsp.EfCore.Catalog;
using PortalModels.Catalog.CatalogCategories;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PortalAsp.Tests.Controllers.Catalog.CatalogCategories
{
    public class CatalogMainCategoryControllerTests
    {
        [Theory]
        [ClassData(typeof(BadPostRequestTestData))]
        public async Task PostMainCategory_DropsBadRequestOnInvalidMainCategory(CatalogMainCategory mainCategory, string resultType)
        {
            //Arrange
            Mock<CatalogContext> mock = new Mock<CatalogContext>(new DbContextOptions<CatalogContext>());
            DbSet<CatalogMainCategory> dbSet = TestHelper.GetQueryableMockDbSet(new CatalogMainCategory
            {
                CatalogMainCategoryId = 1,
                Name = "Initial Main Category",
                ImageAddress = "httpfsdaf.png"
            });
            var mockObj = mock.Object;
            mockObj.CatalogMainCategories = dbSet;
            CatalogMainCategoryController controller = new CatalogMainCategoryController(mockObj);

            //Act
            IActionResult postResult = await controller.PostMainCategory(mainCategory);

            //Assert
            Assert.Equal(resultType, postResult.GetType().Name);
        }

        public class BadPostRequestTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Control
                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Test Category",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "OkResult" };

                //Empty object
                yield return new object[] { new CatalogMainCategory(), "BadRequestObjectResult" };

                //Id
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 3,
                    Name = "Test Category",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "BadRequestObjectResult" };

                //ImageAddress
                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Test Category",
                    ImageAddress = "http://httpf/sdaf"
                }, "BadRequestObjectResult" };
                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Test Category",
                    ImageAddress = "  "
                }, "BadRequestObjectResult" };
                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Test Category"
                }, "BadRequestObjectResult" };

                //Name
                yield return new object[] { new CatalogMainCategory
                {
                    ImageAddress = "http://httpf/sdaf.png"
                }, "BadRequestObjectResult" };
                yield return new object[] { new CatalogMainCategory
                {
                    Name = "   ",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "BadRequestObjectResult" };

                //Name case insensitive exists
                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Initial main category",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "BadRequestObjectResult" };
                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Initial main category",
                    ImageAddress = "http://http/fsdaf.png"
                }, "BadRequestObjectResult" };
                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Initial Cain Category",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "OkResult" };

                //SubCategories
                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Test Category",
                    ImageAddress = "http://http/fsdaf.png",
                    SubCategories = new List<CatalogSubCategory>()
                }, "OkResult" };
                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Test Category",
                    ImageAddress = "http://http/fsdaf.png",
                    SubCategories = new List<CatalogSubCategory> {new CatalogSubCategory()}
                }, "BadRequestObjectResult" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(BadPutRequestTestData))]
        public async Task PutMainCategory_DropsBadRequestOnInvalidMainCategory(CatalogMainCategory mainCategory, string resultType)
        {
            //Arrange
            Mock<CatalogContext> mock = new Mock<CatalogContext>(new DbContextOptions<CatalogContext>());
            DbSet<CatalogMainCategory> dbSet = TestHelper.GetQueryableMockDbSet(new CatalogMainCategory
            {
                CatalogMainCategoryId = 23445,
                Name = "Main Category",
                ImageAddress = "httpfsdaf.png"
            }, new CatalogMainCategory
            {
                CatalogMainCategoryId = 2,
                Name = "Initial Main Category",
                ImageAddress = "httpfsdaf.png"
            });
            var mockObj = mock.Object;
            mockObj.CatalogMainCategories = dbSet;
            CatalogMainCategoryController controller = new CatalogMainCategoryController(mockObj);

            //Act
            IActionResult postResult = await controller.PutMainCategory(mainCategory);

            //Assert
            Assert.Equal(resultType, postResult.GetType().Name);
        }

        public class BadPutRequestTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Control
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 23445,
                    Name = "Test Category",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "OkResult" };

                //Empty object
                yield return new object[] { new CatalogMainCategory(), "BadRequestObjectResult" };

                //Id
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 2344,
                    Name = "Test Category",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "BadRequestObjectResult" };


                //ImageAddress
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 23445,
                    Name = "Test Category",
                    ImageAddress = "http://httpf/sdaf"
                }, "BadRequestObjectResult" };
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 23445,
                    Name = "Test Category",
                    ImageAddress = "  "
                }, "BadRequestObjectResult" };
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 23445,
                    Name = "Test Category"
                }, "BadRequestObjectResult" };

                //Name
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 23445,
                    ImageAddress = "http://httpf/sdaf.png"
                }, "BadRequestObjectResult" };
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 23445,
                    Name = "   ",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "BadRequestObjectResult" };

                //SubCategories
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 23445,
                    Name = "Test Category",
                    ImageAddress = "http://http/fsdaf.png",
                    SubCategories = new List<CatalogSubCategory>()
                }, "OkResult" };
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 23445,
                    Name = "Test Category",
                    ImageAddress = "http://http/fsdaf.png",
                    SubCategories = new List<CatalogSubCategory> {new CatalogSubCategory()}
                }, "BadRequestObjectResult" };

                //Name case insensitive exists
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 23445,
                    Name = "Initial main category",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "BadRequestObjectResult" };
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 2,
                    Name = "Initial main category",
                    ImageAddress = "http://http/fsdaf.png"
                }, "OkResult" };
                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 23445,
                    Name = "Initial Cain Category",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "OkResult" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
