using DelpinBooking.Controllers;
using DelpinBooking.Data;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using DelpinBooking.Controllers;
using Moq;

namespace DelpinTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestGet()
        {
            var booking = new Mock<DelpinBookingContext>();
            var controller = new BookingsController(booking.Object);
            //var result = booking.Details(1) as ViewResult;
            //Assert.AreEqual("Details", result.ViewName);

        }


    }
}