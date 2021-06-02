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
    public class DeleteConfirmed
    {
        [TestMethod]
        public async Task MachineDeleteConfirmed_RedirectsToActionIndex()
        {
            // Arrange
            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup(m => m.Delete(1).Result).Returns(new Machine());
            MachinesController machinesController = new MachinesController(machineMock.Object, null);

            // Act
            var actionResultTask = await machinesController.DeleteConfirmed(1);
            var redirectToActionResult = actionResultTask as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(redirectToActionResult);
            Assert.IsInstanceOfType(actionResultTask, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod]
        public async Task MachineDeleteConfirmed_MachineDeletedWasNull_RedirectsToActionDelete()
        {
            // Arrange
            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup<Task<Machine>>(m => m.Delete(1)).ReturnsAsync((Machine)null);
            MachinesController machinesController = new MachinesController(machineMock.Object, null);

            // Act
            var actionResultTask = await machinesController.DeleteConfirmed(1);
            var redirectToActionResult = actionResultTask as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(redirectToActionResult);
            Assert.IsInstanceOfType(actionResultTask, typeof(RedirectToActionResult));
            Assert.AreEqual("Delete", redirectToActionResult.ActionName);
        }
    }
}