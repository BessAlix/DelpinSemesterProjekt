using DelpinBooking.Classes;
using DelpinBooking.Controllers;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using DelpinBooking.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace DelpinBookingProject.MachineControllerTest
{
    [TestClass]
    public class AddToCart
    {
        [TestMethod]
        public async Task MachineAddToCart_RedirectsToActionIndex()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            var sessionMock = new Mock<ISession>();
            httpContextMock.Setup(c => c.Session).Returns(sessionMock.Object);
            var controllerContext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            var machineMock = new Mock<IHttpClientHandler<Machine>>();
            machineMock.Setup(m => m.Get(1).Result).Returns(new Machine());
            MachinesController machinesController = new MachinesController(machineMock.Object, null);
            machinesController.ControllerContext = controllerContext;

            // Act
            var actionResultTask = await machinesController.AddToCart(1);
            var redirectToActionResult = actionResultTask as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(actionResultTask);
            Assert.IsNotNull(redirectToActionResult);
            Assert.IsInstanceOfType(actionResultTask, typeof(RedirectToActionResult));
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }
    }
}