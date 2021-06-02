using DelpinBooking.Controllers;
using DelpinBooking.Models;
using DelpinBooking.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DelpinBookingProject.MachineControllerTest
{
    [TestClass]
    public class EditPost
    {
        [TestMethod]
        public async Task MachineEditPost_RedirectsToActionIndex()
        {
            // Arrange
            Machine fakeMachine = new Machine
            {
                Id = 1
            };

            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup(m => m.Get(1).Result).Returns(new Machine());
            machineMock.Setup(m => m.Update(fakeMachine).Result).Returns(new Dictionary<string, string>());
            MachinesController machinesController = new MachinesController(machineMock.Object, null);

            // Act
            var actionResultTask = await machinesController.Edit(1, fakeMachine);
            var redirectToActionResult = actionResultTask as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(redirectToActionResult);
            Assert.IsInstanceOfType(actionResultTask, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public async Task MachinesEditPost_RequestIdAndMachineIdNotTheSame_ReturnsNotFound()
        {
            // Arrange
            Machine fakeMachine = new Machine
            {
                Id = 1
            };

            MachinesController machinesController = new MachinesController(null, null);

            // Act
            var actionResultTask = await machinesController.Edit(0, fakeMachine);

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsInstanceOfType(actionResultTask, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task MachineEditPost_MachineToUpdateWasNull_ReturnsEditView()
        {
            // Arrange
            Machine fakeMachine = new Machine
            {
                Id = 1
            };

            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup<Task<Machine>>(m => m.Get(1)).ReturnsAsync((Machine)null);
            MachinesController machinesController = new MachinesController(machineMock.Object, null);

            // Act
            var actionResultTask = await machinesController.Edit(1, fakeMachine);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Edit", viewResult.ViewName);
        }

        [TestMethod]
        public async Task MachineEditPost_UpdateHadConcurrencyErrors_ReturnsEditView()
        {
            // Arrange
            Machine fakeMachine = new Machine
            {
                Id = 1
            };

            Dictionary<string, string> fakeErrors = new Dictionary<string, string>();
            fakeErrors.Add("key", "value");

            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup(m => m.Get(1).Result).Returns(fakeMachine);
            machineMock.Setup(m => m.Update(fakeMachine).Result).Returns(fakeErrors);
            MachinesController machinesController = new MachinesController(machineMock.Object, null);

            // Act
            var actionResultTask = await machinesController.Edit(1, fakeMachine);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Edit", viewResult.ViewName);
        }

        [TestMethod]
        public async Task MachineEditPost_UpdateHadConcurrencyErrors_ReturnsMachineToUpdate()
        {
            // Arrange
            Machine fakeMachine = new Machine
            {
                Id = 1
            };

            Dictionary<string, string> fakeErrors = new Dictionary<string, string>();
            fakeErrors.Add("key", "value");

            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup(m => m.Get(1).Result).Returns(fakeMachine);
            machineMock.Setup(m => m.Update(fakeMachine).Result).Returns(fakeErrors);
            MachinesController machinesController = new MachinesController(machineMock.Object, null);

            // Act 
            var actionResultTask = await machinesController.Edit(1, fakeMachine);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(fakeMachine, viewResult.Model);
        }
    }
}
