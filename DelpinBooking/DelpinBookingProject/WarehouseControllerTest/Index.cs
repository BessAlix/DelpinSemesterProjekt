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
    public class Index

    {
        [TestMethod]
        public async Task WarehouseIndex_ReturnChooseWarehouseView()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(m => m.User.IsInRole("Admin")).Returns(false);

            var Controllercontext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));
            string expectedinput = "page=1&size=10";
            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            WarehouseMock.Setup(w => w.GetAll(expectedinput).Result).Returns(new List<Warehouse>());
            WarehousesController warehousescontroller = new WarehousesController(WarehouseMock.Object);

            warehousescontroller.ControllerContext = Controllercontext;

            // Act
            var actionResultTask = await warehousescontroller.Index(new WarehouseQueryParameters());
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Index", viewResult.ViewName);
        }
        [TestMethod]
        public async Task WarehouseIndex_GetsListOfWarehouses_ReturnListContainingSameWarehouses()
        {
            // Arrange
            List<Warehouse> warehouses = new List<Warehouse>()
            {
                new Warehouse
                {
                    Id = 1,
                    City = "Vejle",
                    PostCode = 1234
                },
                new Warehouse
                {
                    Id = 2,
                    City = "Odense",
                    PostCode = 5000
                },

                new Warehouse
                {
                    Id = 3,
                    City = "Fredericia",
                    PostCode = 5000
                }
            };

            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            var httpContext = new Mock<HttpContext>();
            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(),
                new ControllerActionDescriptor()));

            string expectedinput = "page=1&size=10";
            WarehouseMock.Setup(w => w.GetAll(expectedinput).Result).Returns(warehouses);
            WarehousesController warehousescontroller = new WarehousesController(WarehouseMock.Object);

            warehousescontroller.ControllerContext = context;

            // Act
            var actionResultTask = await warehousescontroller.Index(new WarehouseQueryParameters());
            var viewResult = actionResultTask as ViewResult;
            var resultList = viewResult.Model as List<Warehouse>;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreEqual("Odense", resultList[1].City);
            Assert.AreEqual("Vejle", resultList[0].City);
            Assert.AreEqual("Fredericia", resultList[2].City);
        }

        [TestMethod]
        public async Task WarehouseIndex_ReturnIndexView()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(m => m.User.IsInRole("Admin")).Returns(true);

            var Controllercontext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            string expectedinput = "page=1&size=10";
            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            WarehouseMock.Setup(w => w.GetAll(expectedinput).Result).Returns(new List<Warehouse>());
            WarehousesController warehousescontroller = new WarehousesController(WarehouseMock.Object);

            warehousescontroller.ControllerContext = Controllercontext;

            // Act
            var actionResultTask = await warehousescontroller.Index(new WarehouseQueryParameters());
            var viewResult = actionResultTask as ViewResult;

            //Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Index", viewResult.ViewName);
        }

    }
}
