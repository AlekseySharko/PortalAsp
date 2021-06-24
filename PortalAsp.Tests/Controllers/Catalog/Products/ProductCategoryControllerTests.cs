using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PortalAsp.Controllers.Catalog.Products;
using PortalAsp.EfCore.Catalog;
using PortalModels;
using PortalModels.Catalog.CatalogCategories;
using PortalModels.Catalog.Products;
using Xunit;

namespace PortalAsp.Tests.Controllers.Catalog.Products
{
    public class ProductCategoryControllerTests
    {
        [Theory]
        [ClassData(typeof(BadRequestTestData))]
        public void PostManufacturer_DropsBadRequestOnInvalidProductCategoryOrSubCategoryId(ProductCategory productCategory, int subCategoryId, string resultType)
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
            IActionResult postResult = controller.PostProductCategory(productCategory, subCategoryId);

            //Assert
            Assert.Equal(resultType, postResult.GetType().Name);
        }

        public class BadRequestTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new ProductCategory
                    {
                        Name = "Name"
                    },
                    2,
                    "OkResult"
                };

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

                yield return new object[]
                {
                    new ProductCategory
                    {
                        Name = "Name"
                    },
                    0,
                    "BadRequestObjectResult"
                };

                yield return new object[] { new ProductCategory(), 2,  "BadRequestObjectResult" };

                yield return new object[]
                {
                    new ProductCategory
                    {
                        Name = "  "
                    },
                    2,
                    "BadRequestObjectResult"
                };

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
    }
}
