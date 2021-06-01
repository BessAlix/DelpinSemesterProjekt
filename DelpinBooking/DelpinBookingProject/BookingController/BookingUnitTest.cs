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

namespace DelpinBookingProject
{
    [TestClass]
    public class BookingIndex
    {
        [TestMethod]
        public async Task BookingIndex_GetsListOfBookings_ReturnListContainingSameBookings()
        {
            // Arrange
            List<Booking> bookings = new List<Booking>()
            {
                new Booking()
                {
                    Id = 1,
                },
                new Booking()
                {
                    Id = 2,
                }
            };

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(m => m.User.IsInRole("Admin")).Returns(true);
            var Controllercontext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            string expectedinput = "page=1&size=10";
            var BookingMock = new Mock<IHttpClientHandler<Booking>>();
            BookingMock.Setup(w => w.GetAll(expectedinput).Result).Returns(bookings);
            BookingsController bookingscontroller = new BookingsController(BookingMock.Object);

            bookingscontroller.ControllerContext = Controllercontext;

            // Act
            var actionResultTask = await bookingscontroller.Index(new BookingQueryParameters());
            var viewResult = actionResultTask as ViewResult;
            var resultList = viewResult.Model as List<Booking>;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreEqual(2, resultList[1].Id);
            Assert.AreEqual(1, resultList[0].Id);
        }
        [TestMethod]
        public async Task BookingIndex_ReturnIndexView()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(m => m.User.IsInRole("Admin")).Returns(true);

            var Controllercontext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                new ControllerActionDescriptor()));

            string expectedinput = "page=1&size=10";
            var BookingMock = new Mock<IHttpClientHandler<Booking>>();
            BookingMock.Setup(w => w.GetAll(expectedinput).Result).Returns(new List<Booking>());
            BookingsController bookingscontroller = new BookingsController(BookingMock.Object);
            bookingscontroller.ControllerContext = Controllercontext;

            // Act
            var actionResultTask = await bookingscontroller.Index(new BookingQueryParameters());
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Index", viewResult.ViewName);
        }
    }
    [TestClass]
    public class BookingDelete
    {
        [TestMethod]
        public async Task BookingDeleteConfirmed_returnDeleted()
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
            BookingMock.Setup(w => w.Delete(1).Result).Returns(fakebooking);
            BookingsController bookingscontroller = new BookingsController(BookingMock.Object);
            bookingscontroller.ControllerContext = Controllercontext;

            // Act
            var actionResultTask = await bookingscontroller.DeleteConfirmed(fakebooking);
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("DeleteCompleted", viewResult.ViewName);
            Assert.AreEqual(fakebooking, viewResult.Model);
        }

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

        [TestClass]
        public class BookingEdit
        {
            public async Task BookinEdit_ReturnIndexView()
            {
                // Arrange
                var fakebooking = new Booking()
                {
                    Id = 1,
                };

                var httpContextMock = new Mock<HttpContext>();
                httpContextMock.Setup(m => m.User.IsInRole("Admin")).Returns(true);

                var Controllercontext = new ControllerContext(new ActionContext(httpContextMock.Object, new RouteData(),
                    new ControllerActionDescriptor()));

                var BookingMock = new Mock<IHttpClientHandler<Booking>>();
                BookingMock.Setup(w => w.Get(1).Result).Returns(new Booking());
                BookingsController bookingscontroller = new BookingsController(BookingMock.Object);
                bookingscontroller.ControllerContext = Controllercontext;

                // Act
                var actionResultTask = await bookingscontroller.Edit(1, fakebooking);
                var viewResult = actionResultTask as ViewResult;

                // Assert
                Assert.IsNotNull(viewResult);
                Assert.AreEqual("Index", viewResult.ViewName);
            }

        }


    }

}
