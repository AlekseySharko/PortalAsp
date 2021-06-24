﻿using System.Collections;
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
        [ClassData(typeof(BadRequestTestData))]
        public void PostManufacturer_DropsBadRequestOnInvalidManufacturer(Manufacturer manufacturer, string resultType)
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

        public class BadRequestTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new Manufacturer
                {
                    Name = "Name",
                    Country = "Country"
                }, "OkResult" };

                yield return new object[] { new Manufacturer(), "BadRequestObjectResult" };

                yield return new object[] { new Manufacturer
                {
                    Name = "Name",
                    Country = "Country",
                    ManufacturerId = 123
                }, "BadRequestObjectResult" };

                yield return new object[] { new Manufacturer
                {
                    Name = " ",
                    Country = "Country"
                }, "BadRequestObjectResult" };

                yield return new object[] { new Manufacturer
                {
                    Name = "Name",
                    Country = " "
                }, "BadRequestObjectResult" };

                yield return new object[] { new Manufacturer
                {
                    Name = " ",
                    Country = " "
                }, "BadRequestObjectResult" };
                yield return new object[] { new Manufacturer
                {
                    Name = "Initial manufacturer",
                    Country = "Country"
                }, "BadRequestObjectResult" };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}