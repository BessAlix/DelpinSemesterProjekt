using DelpinBooking.Classes;
using DelpinBooking.Controllers;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DelpinBooking.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace DelpinBookingProject.MachineControllerTest
{
    [TestClass]
    public class Index
    {
        [TestMethod]
        public async Task MachineIndex_ReturnsListOfMachinesForView()
        {
            // Arrange
            List<Machine> fakeMachines = new List<Machine>()
            {
                new Machine
                {
                    Id = 1,
                    Name = "Spanskrør",
                },
                new Machine
                {
                    Id = 2,
                    Name = "Sømpistol",
                }
            };

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User.IsInRole("Admin")).Returns(true);
            var controllerContext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            var warehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            warehouseMock.Setup(w => w.GetAll("").Result).Returns(new List<Warehouse>());
            WarehousesController warehousesController = new WarehousesController(warehouseMock.Object);

            string expectedInput = "page=1&size=10";
            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup(m => m.GetAll(expectedInput).Result).Returns(fakeMachines);
            MachinesController machinesController = new MachinesController(machineMock.Object, warehousesController);
            machinesController.ControllerContext = controllerContext;

            // Act
            var actionResultTask = await machinesController.Index(new MachineQueryParameters());
            var viewResult = actionResultTask as ViewResult;
            var resultList = viewResult.Model as List<Machine>;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(resultList);
            Assert.AreEqual(fakeMachines, resultList);
            Assert.AreEqual(1, resultList[0].Id);
            Assert.AreEqual("Sømpistol", resultList[1].Name);
        }

        [TestMethod]
        public async Task MachineIndex_ReturnsIndexView()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User.IsInRole("Admin")).Returns(true);
            var controllerContext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            var warehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            warehouseMock.Setup(w => w.GetAll("").Result).Returns(new List<Warehouse>());
            WarehousesController warehousesController = new WarehousesController(warehouseMock.Object);

            string expectedInput = "page=1&size=10";
            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup(m => m.GetAll(expectedInput).Result).Returns(new List<Machine>());
            MachinesController machinesController = new MachinesController(machineMock.Object, warehousesController);
            machinesController.ControllerContext = controllerContext;

            // Act
            var actionResultTask = await machinesController.Index(new MachineQueryParameters());
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Index", viewResult.ViewName);
        }

        [TestMethod]
        public async Task MachineIndex_UserIsNotAdmin_ReturnsChooseMachineView()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User.IsInRole("Admin")).Returns(false);
            var controllerContext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            var warehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            warehouseMock.Setup(w => w.GetAll("").Result).Returns(new List<Warehouse>());
            WarehousesController warehousesController = new WarehousesController(warehouseMock.Object);

            string expectedInput = "page=1&size=10";
            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup(m => m.GetAll(expectedInput).Result).Returns(new List<Machine>());
            MachinesController machinesController = new MachinesController(machineMock.Object, warehousesController);
            machinesController.ControllerContext = controllerContext;

            // Act
            var actionResultTask = await machinesController.Index(new MachineQueryParameters());
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("ChooseMachines", viewResult.ViewName);
        }
    }
}
