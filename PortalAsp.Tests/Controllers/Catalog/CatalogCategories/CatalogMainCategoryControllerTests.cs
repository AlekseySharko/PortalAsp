using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PortalAsp.Controllers.Catalog.CatalogCategories;
using PortalAsp.EfCore.Catalog;
using PortalModels.Catalog.CatalogCategories;
using Xunit;

namespace PortalAsp.Tests.Controllers.Catalog.CatalogCategories
{
    public class CatalogMainCategoryControllerTests
    {
        [Theory]
        [ClassData(typeof(BadRequestTestData))]
        public void PostManufacturer_DropsBadRequestOnInvalidMainCategory(CatalogMainCategory mainCategory, string resultType)
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
            IActionResult postResult = controller.PostMainCategory(mainCategory);

            //Assert
            Assert.Equal(resultType, postResult.GetType().Name);
        }

        public class BadRequestTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Test Category",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "OkResult" };

                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 3,
                    Name = "Test Category",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "BadRequestObjectResult" };

                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Test Category",
                    ImageAddress = "http://httpf/sdaf"
                }, "BadRequestObjectResult" };
                yield return new object[] { new CatalogMainCategory(), "BadRequestObjectResult" };

                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Test Category",
                    ImageAddress = "  "
                }, "BadRequestObjectResult" };

                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Test Category"
                }, "BadRequestObjectResult" };

                yield return new object[] { new CatalogMainCategory
                {
                    ImageAddress = "http://httpf/sdaf.png"
                }, "BadRequestObjectResult" };

                yield return new object[] { new CatalogMainCategory
                {
                    Name = "   ",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "BadRequestObjectResult" };

                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Initial main category",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "BadRequestObjectResult" };

                yield return new object[] { new CatalogMainCategory
                {
                    Name = "Initial Cain Category",
                    ImageAddress = "http://httpf/sdaf.png"
                }, "OkResult" };

                yield return new object[] { new CatalogMainCategory
                {
                    CatalogMainCategoryId = 3,
                    Name = "Initial main category",
                    ImageAddress = "http://http/fsdaf.png"
                }, "BadRequestObjectResult" };

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
    }
}
