using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
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

namespace DelpinBookingProject.BookingControllerTest
{
    [TestClass]
    public class Delete
    {

        [TestMethod]
        public async Task BookingDelete_ReturnDeleteView()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(m => m.User.IsInRole("Admin")).Returns(true);
            var Controllercontext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            var fakebooking = new Booking()
            {
                Id = 1
            };
            var BookingMock = new Mock<IHttpClientHandler<Booking>>();
            BookingMock.Setup(w => w.Get(1).Result).Returns(fakebooking);
            BookingsController bookingscontroller = new BookingsController(BookingMock.Object);
            bookingscontroller.ControllerContext = Controllercontext;

            // Act
            var actionResultTask = await bookingscontroller.Delete(1);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Delete", viewResult.ViewName);
        }

        [TestMethod]
        public async Task BookingDelete_ReturnNotAuthorizedView()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            var mockIdentity = new Mock<IIdentity>();
            httpContextMock.Setup(x => x.User.Identity).Returns(mockIdentity.Object);
            mockIdentity.Setup(x => x.Name).Returns("222");

            var Controllercontext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            var fakebooking = new Booking()
            {
                Id = 1,
                Customer = "222"
            };
            var BookingMock = new Mock<IHttpClientHandler<Booking>>();
            BookingMock.Setup(w => w.Get(1).Result).Returns(fakebooking);
            BookingsController bookingscontroller = new BookingsController(BookingMock.Object);
            bookingscontroller.ControllerContext = Controllercontext;

            // Act
            var actionResultTask = await bookingscontroller.Delete(1);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("NotAuthorized", viewResult.ViewName);
        }

        // Virker ikke
        [TestMethod]
        public async Task BookingDelete_ReturnNotNotFound()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(m => m.User.IsInRole("Admin")).Returns(true);
            var Controllercontext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            Booking fakebooking = null;
            var BookingMock = new Mock<IHttpClientHandler<Booking>>();
            BookingMock.Setup(w => w.Get(1).Result).Returns(fakebooking);
            BookingsController bookingscontroller = new BookingsController(BookingMock.Object);
            bookingscontroller.ControllerContext = Controllercontext;

            // Act
            var actionResultTask = await bookingscontroller.Delete(1);


            // Assert
            Assert.IsInstanceOfType(actionResultTask, typeof(NotFoundResult));
        }
    }
}
