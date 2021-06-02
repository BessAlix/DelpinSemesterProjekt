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
    public class Delete
    {
        [TestMethod]
        public async Task WarehouseDelete_ReturnIndexView()
        {
            // Arrange
            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            var httpContext = new Mock<HttpContext>();
            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(),
                new ControllerActionDescriptor()));
            var fakewarehouse = new Warehouse()
            {
                Id = 1
            };

            // Act
            httpContext.Setup(m => m.User.IsInRole("Admin")).Returns(true);
            WarehousesController warehousescontroller = new WarehousesController(WarehouseMock.Object);
            WarehouseMock.Setup(m => m.Get(1).Result).Returns(fakewarehouse);
            warehousescontroller.ControllerContext = context;
            var actionResultTask = await warehousescontroller.Delete(1);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Delete", viewResult.ViewName);
        }
    }
}