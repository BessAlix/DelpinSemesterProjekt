using DelpinBooking.Classes;
using DelpinBooking.Controllers;
using DelpinBooking.Models;
using DelpinBooking.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DelpinBookingProject
{
    [TestClass]
    public class MachineIndex
    {
        [TestMethod]
        public async Task MachineIndex_GetsListOfMachines_ReturnListContainingSameMachines()
        {
            // Arrange
            List<Machine> machines = new List<Machine>()
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
            httpContextMock.Setup(m => m.User.IsInRole("Admin")).Returns(true);
            var Controllercontext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));


            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            WarehouseMock.Setup(w => w.GetAll("").Result).Returns(new List<Warehouse>());
            WarehousesController warehousescontroller = new WarehousesController(WarehouseMock.Object);

            string expectedinput = "page=1&size=10";
            var MachineMock = new Mock<IHttpClientHandler<Machine>>();
            MachineMock.Setup(m => m.GetAll(expectedinput).Result).Returns(machines);
            MachinesController machinescontroller = new MachinesController(MachineMock.Object, warehousescontroller);

            machinescontroller.ControllerContext = Controllercontext;

            // Act
            var actionResultTask = await machinescontroller.Index(new MachineQueryParameters());
            var viewResult = actionResultTask as ViewResult;
            var resultList = viewResult.Model as List<Machine>;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreEqual("Sømpistol", resultList[1].Name);
            Assert.AreEqual("Spanskrør", resultList[0].Name);
        }

        [TestMethod]
        public async Task MachineIndex_ReturnIndexView()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(m => m.User.IsInRole("Admin")).Returns(true);

            var Controllercontext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            WarehouseMock.Setup(w => w.GetAll("").Result).Returns(new List<Warehouse>());
            WarehousesController warehousescontroller = new WarehousesController(WarehouseMock.Object);


            var MachineMock = new Mock<IHttpClientHandler<Machine>>();
            MachineMock.Setup(m => m.GetAll("page=1&size=10").Result).Returns(new List<Machine>());
            MachinesController machinescontroller = new MachinesController(MachineMock.Object, warehousescontroller);
            machinescontroller.ControllerContext = Controllercontext;

            // Act
            var actionResultTask = await machinescontroller.Index(new MachineQueryParameters());
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Index", viewResult.ViewName);
        }

        [TestMethod]
        public async Task MachineIndex_ReturnChooseMachineView()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(m => m.User.IsInRole("Admin")).Returns(false);

            var Controllercontext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            WarehouseMock.Setup(w => w.GetAll("").Result).Returns(new List<Warehouse>());
            WarehousesController warehousescontroller = new WarehousesController(WarehouseMock.Object);


            var MachineMock = new Mock<IHttpClientHandler<Machine>>();
            MachineMock.Setup(m => m.GetAll("page=1&size=10").Result).Returns(new List<Machine>());
            MachinesController machinescontroller = new MachinesController(MachineMock.Object, warehousescontroller);
            machinescontroller.ControllerContext = Controllercontext;


            // Act
            var actionResultTask = await machinescontroller.Index(new MachineQueryParameters());
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("ChooseMachines", viewResult.ViewName);
        }
    }

    [TestClass]
    public class MachineDelete
    {
        [TestMethod]
        public async Task MachineDeleteConfirmed_returnDeletedMachine()
        {
            // Arrange
            var MachineMock = new Mock<IHttpClientHandler<Machine>>();
            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            var httpContext = new Mock<HttpContext>();
            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(),
                new ControllerActionDescriptor()));
            var fakemachine = new Machine()
            {
                Id = 1
            };

            // Act
            httpContext.Setup(m => m.User.IsInRole("Admin")).Returns(true);
            MachinesController machinescontroller = new MachinesController(MachineMock.Object, null);
            MachineMock.Setup(m => m.Delete(1).Result).Returns(fakemachine);
            machinescontroller.ControllerContext = context;
            var actionResultTask = await machinescontroller.DeleteConfirmed(1);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("DeleteCompleted", viewResult.ViewName);
            Assert.AreEqual(fakemachine, viewResult.Model);
        }

        [TestMethod]
        public async Task MachineDelete_ReturnIndexView()
        {
            // Arrange
            var MachineMock = new Mock<IHttpClientHandler<Machine>>();
            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            var httpContext = new Mock<HttpContext>();
            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(),
                new ControllerActionDescriptor()));
            var fakemachine = new Machine()
            {
                Id = 1
            };

            // Act
            httpContext.Setup(m => m.User.IsInRole("Admin")).Returns(true);
            MachinesController machinescontroller = new MachinesController(MachineMock.Object, null);
            MachineMock.Setup(m => m.Get(1).Result).Returns(fakemachine);
            machinescontroller.ControllerContext = context;
            var actionResultTask = await machinescontroller.Delete(1);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Delete", viewResult.ViewName);
        }

    }
}
