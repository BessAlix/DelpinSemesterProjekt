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
    public class Edit
    {
        [TestMethod]
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
    
        
    
