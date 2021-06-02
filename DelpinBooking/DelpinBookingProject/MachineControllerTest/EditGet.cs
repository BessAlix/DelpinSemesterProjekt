using DelpinBooking.Controllers;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using DelpinBooking.Models.Interfaces;

namespace DelpinBookingProject.MachineControllerTest
{
    [TestClass]
    public class EditGet
    {
        [TestMethod]
        public async Task MachineCreateGet_ReturnsEditView()
        {
            // Arrange
            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup(m => m.Get(1).Result).Returns(new Machine());
            MachinesController machinesController = new MachinesController(machineMock.Object, null);

            // Act
            var actionResultTask = await machinesController.Edit(1);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Edit", viewResult.ViewName);
        }

        [TestMethod]
        public async Task MachineCreateGet_ReturnsMachineToEdit()
        {
            // Arrange
            Machine fakeMachine = new Machine
            {
                Id = 1
            };

            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup(m => m.Get(1).Result).Returns(fakeMachine);
            MachinesController machinesController = new MachinesController(machineMock.Object, null);

            // Act
            var actionResultTask = await machinesController.Edit(1);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(fakeMachine, viewResult.Model);
        }

        [TestMethod]
        public async Task MachineCreateGet_NoSuchMachineExists_ReturnsNotFound()
        {
            // Arrange
            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup<Task<Machine>>(m => m.Get(1)).ReturnsAsync((Machine)null);
            MachinesController machinesController = new MachinesController(machineMock.Object, null);

            // Act
            var actionResultTask = await machinesController.Edit(1);

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsInstanceOfType(actionResultTask, typeof(NotFoundResult));
        }
    }
}
