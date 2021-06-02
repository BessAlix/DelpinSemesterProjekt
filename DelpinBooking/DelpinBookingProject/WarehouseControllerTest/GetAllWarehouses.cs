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
    public class GetAllWarehouses
    {

        [TestMethod]
        public async Task WarehouseGetAllWarehouses_returnAllWarehouses()
        {
            // Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.IsInRole("Admin")).Returns(true);
            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(),
                new ControllerActionDescriptor()));

            string expectedinput = "page=1&size=10";
            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            WarehousesController warehousescontroller = new WarehousesController(WarehouseMock.Object);
            warehousescontroller.ControllerContext = context;


            List<Warehouse> warehouses = new List<Warehouse>()
             {
                new Warehouse
                {

                    Id = 1
                },
                new Warehouse
                {

                    Id = 2

                },

                new Warehouse
                {

                    Id = 3

                }
            };

            WarehouseMock.Setup(w => w.GetAll(expectedinput).Result).Returns(warehouses);

            // Act
            var actionResultTask = await warehousescontroller.GetAllWarehouses();

            // Assert
            Assert.AreEqual(actionResultTask, warehouses);
            Assert.IsInstanceOfType(actionResultTask,typeof(List<Machine>));
        }
    }
}