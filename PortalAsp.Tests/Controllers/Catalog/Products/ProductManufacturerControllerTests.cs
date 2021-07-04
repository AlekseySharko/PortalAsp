using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PortalAsp.Controllers.Catalog.Products;
using PortalAsp.EfCore.Catalog;
using PortalAsp.EfCore.Catalog.Repositories;
using PortalModels.Catalog.Products;
using Xunit;

namespace PortalAsp.Tests.Controllers.Catalog.Products
{
    public class ProductManufacturerControllerTests
    {
        private static ProductManufacturerController GetController()
        {
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
            return new ProductManufacturerController(new EfManufacturerRepository(mockObj));
        }

        [Theory]
        [ClassData(typeof(BadPostRequestTestData))]
        public async Task PostManufacturer_DropsBadRequestOnAddInvalidManufacturer(Manufacturer manufacturer, string resultType)
        {
            //Arrange
            ProductManufacturerController controller = GetController();

            //Act
            IActionResult postResult = await controller.PostManufacturer(manufacturer);

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
                    ManufacturerId = 5624
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
        public async Task PutManufacturer_DropsBadRequestOnPutInvalidManufacturer(Manufacturer manufacturer, string resultType)
        {
            //Arrange
            ProductManufacturerController controller = GetController();

            //Act
            IActionResult postResult = await controller.PutManufacturer(manufacturer);

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
        public async Task PostManufacturer_DropsBadRequestOnInvalidManufacturer(long manufacturerId, string resultType)
        {
            //Arrange
            ProductManufacturerController controller = GetController();

            //Act
            IActionResult postResult = await controller.DeleteManufacturer(manufacturerId);

            //Assert
            Assert.Equal(resultType, postResult.GetType().Name);
        }

        public class BadDeleteRequestTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                //Control
                yield return new object[] { 5624, "OkResult" };
                yield return new object[] { 562, "BadRequestObjectResult" };
                yield return new object[] { 111, "BadRequestObjectResult" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
