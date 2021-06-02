using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DeleteConfirmed
    {
        [TestMethod]
        public async Task BookingDeleteConfirmed_returnDeleted()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(m => m.User.IsInRole("Admin")).Returns(true);

            var controllercontext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            var fakebooking = new Booking()
            {
                Id = 1
            };

            var BookingMock = new Mock<IHttpClientHandler<Booking>>();
            BookingMock.Setup(w => w.Delete(1).Result).Returns(fakebooking);
            BookingsController bookingscontroller = new BookingsController(BookingMock.Object);
            bookingscontroller.ControllerContext = controllercontext;

            // Act
            var actionResultTask = await bookingscontroller.DeleteConfirmed(fakebooking);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("DeleteCompleted", viewResult.ViewName);
            Assert.AreEqual(fakebooking, viewResult.Model);
        }
    }
}
