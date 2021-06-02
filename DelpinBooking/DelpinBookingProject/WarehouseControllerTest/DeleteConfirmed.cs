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
    public class DeleteConfirmed
    {
        [TestMethod]
        public async Task WarehouseDeleteConfirmed_returnDeletedWarehouse()
        {
            // Arrange
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(m => m.User.IsInRole("Admin")).Returns(true);
            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(),
                new ControllerActionDescriptor()));
            var fakewarehouse = new Warehouse()
            {
                Id = 1
            };
            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            WarehouseMock.Setup(m => m.Delete(1).Result).Returns(fakewarehouse);
            WarehousesController warehousescontroller = new WarehousesController(WarehouseMock.Object);
            warehousescontroller.ControllerContext = context;

            // Act
            var actionResultTask = await warehousescontroller.DeleteConfirmed(1);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("DeleteCompleted", viewResult.ViewName);
            Assert.AreEqual(fakewarehouse, viewResult.Model);
        }

        [TestMethod]
        public async Task WarehouseDeleteConfirmed_returnDeletedWarehouseNotFound()
        {
            // Arrange
            Warehouse fakewarehouse = null;
            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            WarehouseMock.Setup<Task<Warehouse>>(m => m.Get(1)).ReturnsAsync((Warehouse)null);
            WarehousesController warehousescontroller = new WarehousesController(WarehouseMock.Object);

            // Act
            var actionResultTask = await warehousescontroller.Delete(1);
            var viewResult = actionResultTask as NotFoundResult;

            // Assert
            Assert.IsInstanceOfType(viewResult, typeof(NotFoundResult));
        }
    }
}