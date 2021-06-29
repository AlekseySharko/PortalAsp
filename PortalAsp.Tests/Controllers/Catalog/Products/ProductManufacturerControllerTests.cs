using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PortalAsp.Controllers.Catalog.Products;
using PortalAsp.EfCore.Catalog;
using PortalModels.Catalog.Products;
using Xunit;

namespace PortalAsp.Tests.Controllers.Catalog.Products
{
    public class ProductManufacturerControllerTests
    {
        [Theory]
        [ClassData(typeof(BadPostRequestTestData))]
        public void PostManufacturer_DropsBadRequestOnAddInvalidManufacturer(Manufacturer manufacturer, string resultType)
        {
            //Arrange
            Mock<CatalogContext> mock = new Mock<CatalogContext>(new DbContextOptions<CatalogContext>());
            DbSet<Manufacturer> dbSet = TestHelper.GetQueryableMockDbSet(new Manufacturer
            {
                ManufacturerId = 1,
                Name = "Initial Manufacturer",
                Country = "USA"
            });
            var mockObj = mock.Object;
            mockObj.Manufacturers = dbSet;
            ProductManufacturerController controller = new ProductManufacturerController(mockObj);

            //Act
            IActionResult postResult = controller.PostManufacturer(manufacturer);

            //Assert
            Assert.Equal(resultType, postResult.GetType().Name);
        }

        public class BadPostRequestTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Control
                yield return new object[] { new Manufacturer
                {
                    Name = "Name",
                    Country = "Country"
                }, "OkResult" };

                //Empty object
                yield return new object[] { new Manufacturer(), "BadRequestObjectResult" };

                //Id
                yield return new object[] { new Manufacturer
                {
                    Name = "Name",
                    Country = "Country",
                    ManufacturerId = 123
                }, "BadRequestObjectResult" };

                //Name
                yield return new object[] { new Manufacturer
                {
                    Name = " ",
                    Country = "Country"
                }, "BadRequestObjectResult" };

                //Country
                yield return new object[] { new Manufacturer
                {
                    Name = "Name",
                    Country = " "
                }, "BadRequestObjectResult" };
                
                //Name case insensitive exists
                yield return new object[] { new Manufacturer
                {
                    Name = "Initial manufacturer",
                    Country = "Country"
                }, "BadRequestObjectResult" };
                yield return new object[] { new Manufacturer
                {
                    Name = "Initial anufacturer",
                    Country = "Country"
                }, "OkResult" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(BadPutRequestTestData))]
        public void PutManufacturer_DropsBadRequestOnPutInvalidManufacturer(Manufacturer manufacturer, string resultType)
        {
            //Arrange
            Mock<CatalogContext> mock = new Mock<CatalogContext>(new DbContextOptions<CatalogContext>());
            DbSet<Manufacturer> dbSet = TestHelper.GetQueryableMockDbSet(new Manufacturer
            {
                ManufacturerId = 5624,
                Name = "Initial Manufacturer",
                Country = "USA",
            }, new Manufacturer
            {
                ManufacturerId = 123,
                Name = "Second Initial Manufacturer",
                Country = "USA",
            });
            var mockObj = mock.Object;
            mockObj.Manufacturers = dbSet;
            ProductManufacturerController controller = new ProductManufacturerController(mockObj);

            //Act
            IActionResult postResult = controller.PutManufacturer(manufacturer);

            //Assert
            Assert.Equal(resultType, postResult.GetType().Name);

        }

        public class BadPutRequestTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Control
                yield return new object[] { new Manufacturer
                {
                    ManufacturerId = 5624,
                    Name = "Name",
                    Country = "Country"
                }, "OkResult" };

                //NoId
                yield return new object[] { new Manufacturer
                {
                    Name = "Name",
                    Country = "Country"
                }, "BadRequestObjectResult" };

                //Empty object
                yield return new object[] { new Manufacturer(), "BadRequestObjectResult" };

                //Id
                yield return new object[] { new Manufacturer
                {
                    Name = "Name",
                    Country = "Country",
                    ManufacturerId = 5624,
                }, "OkResult" };

                //Name
                yield return new object[] { new Manufacturer
                {
                    ManufacturerId = 5624,
                    Name = " ",
                    Country = "Country"
                }, "BadRequestObjectResult" };

                //Country
                yield return new object[] { new Manufacturer
                {
                    ManufacturerId = 5624,
                    Name = "Name",
                    Country = " "
                }, "BadRequestObjectResult" };

                //Name case insensitive exists
                yield return new object[] { new Manufacturer
                {
                    ManufacturerId = 5624,
                    Name = "Second initial manufacturer",
                    Country = "Country"
                }, "BadRequestObjectResult" };
                yield return new object[] { new Manufacturer
                {
                    ManufacturerId = 123,
                    Name = "Second Initial Manufacturer",
                    Country = "Country"
                }, "OkResult" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(BadDeleteRequestTestData))]
        public void PostManufacturer_DropsBadRequestOnInvalidManufacturer(long manufacturerId, string resultType)
        {
            //Arrange
            Mock<CatalogContext> mock = new Mock<CatalogContext>(new DbContextOptions<CatalogContext>());
            DbSet<Manufacturer> dbSet = TestHelper.GetQueryableMockDbSet(new Manufacturer
            {
                ManufacturerId = 321,
                Name = "Initial Manufacturer",
                Country = "USA"
            });
            var mockObj = mock.Object;
            mockObj.Manufacturers = dbSet;
            ProductManufacturerController controller = new ProductManufacturerController(mockObj);

            //Act
            IActionResult postResult = controller.DeleteManufacturer(manufacturerId);

            //Assert
            Assert.Equal(resultType, postResult.GetType().Name);
        }

        public class BadDeleteRequestTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Control
                yield return new object[] { 321 , "OkResult" };
                yield return new object[] { 333, "BadRequestObjectResult" };
                yield return new object[] { 111, "BadRequestObjectResult" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
