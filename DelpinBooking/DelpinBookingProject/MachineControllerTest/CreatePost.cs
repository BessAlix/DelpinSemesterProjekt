using DelpinBooking.Controllers;
using DelpinBooking.Models;
using DelpinBooking.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace DelpinBookingProject.MachineControllerTest
{
    [TestClass]
    public class CreatePost
    {
        [TestMethod]
        public async Task MachineCreatePost_RedirectsToActionIndex()
        {
            // Arrange
            Machine fakeMachine = new Machine
            {
                Id = 1
            };

            var warehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            warehouseMock.Setup(w => w.Get(1).Result).Returns(new Warehouse());
            WarehousesController warehousesController = new WarehousesController(warehouseMock.Object);

            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup(m => m.Create(fakeMachine).Result).Returns(fakeMachine);
            MachinesController machinesController = new MachinesController(machineMock.Object, warehousesController);

            // Act
            var actionResultTask = await machinesController.Create(fakeMachine, 1);
            var redirectToActionResult = actionResultTask as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(redirectToActionResult);
            Assert.IsInstanceOfType(actionResultTask, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public async Task MachineCreatePost_MachineCreatedWasNull_RedirectsToActionCreate()
        {
            // Arrange
            Machine fakeMachine = new Machine
            {
                Id = 1
            };

            var warehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            warehouseMock.Setup(w => w.Get(1).Result).Returns(new Warehouse());
            WarehousesController warehousesController = new WarehousesController(warehouseMock.Object);

            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup<Task<Machine>>(m => m.Create(fakeMachine)).ReturnsAsync((Machine)null);
            MachinesController machinesController = new MachinesController(machineMock.Object, warehousesController);

            // Act
            var actionResultTask = await machinesController.Create(fakeMachine, 1);
            var redirectToActionResult = actionResultTask as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(redirectToActionResult);
            Assert.IsInstanceOfType(actionResultTask, typeof(RedirectToActionResult));
            Assert.AreEqual("Create", redirectToActionResult.ActionName);
        }
    }
}
