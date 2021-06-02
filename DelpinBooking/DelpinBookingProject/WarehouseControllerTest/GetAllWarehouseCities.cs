using System;
using DelpinBooking.Classes;
using DelpinBooking.Controllers;
using DelpinBooking.Data;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using DelpinBooking.Controllers.Handler;
using Xunit;
using DelpinBooking.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;


namespace DelpinBookingProject.WarehouseControllerTest
{
    [TestClass]
    public class GetAllWarehouseCities
    {
        [TestMethod]
        public async Task WarehouseGetAllWarehouseCities_returnAllCities()
        {
            // Arrange
            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            WarehousesController warehousescontroller = new WarehousesController(WarehouseMock.Object);

            List<Warehouse> warehouses = new List<Warehouse>()
             {
                new Warehouse
                {

                    City = "Vejle"
                },
                new Warehouse
                {

                    City = "Odense"

                },

                new Warehouse
                {

                    City = "Fredericia"

                }
            };

            WarehouseMock.Setup(w => w.GetAll("").Result).Returns(warehouses);

            // Act
            var actionResultTask = await warehousescontroller.GetAllWarehouseCities();

            // Assert
            Assert.AreEqual(actionResultTask, warehouses);
            Assert.IsInstanceOfType(actionResultTask, typeof(List<string>));
        }
    }
}