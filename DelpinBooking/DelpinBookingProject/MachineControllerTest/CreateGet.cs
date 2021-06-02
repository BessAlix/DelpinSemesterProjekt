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
    public class CreateGet
    {
        [TestMethod]
        public async Task MachineCreateGet_ReturnsCreateView()
        {
            // Arrange
            var warehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            warehouseMock.Setup(w => w.GetAll("").Result).Returns(new List<Warehouse>());
            WarehousesController warehousesController = new WarehousesController(warehouseMock.Object);

            MachinesController machinesController = new MachinesController(null, warehousesController);

            // Act
            var actionResultTask = await machinesController.Create();
            var viewResult = actionResultTask as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Create", viewResult.ViewName);
        }
    }
}