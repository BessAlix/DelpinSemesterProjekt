using System;
using DelpinBooking.Classes;
using DelpinBooking.Controllers;
using DelpinBooking.Data;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DelpinBooking.Controllers.Handler;
using Xunit;
using DelpinBooking.Models.Interfaces;

namespace DelpinBookingProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void HomeView()
        {
            // Arrange
            var HomeRepository = new Mock<ILogger<HomeController>>();
            HomeController homecontroller = new HomeController(HomeRepository.Object);
            //var actionResultTask = homecontroller.Index();
            //await actionResultTask.Wait();
            // Act
            var viewresult = homecontroller.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(viewresult);
            Assert.AreEqual("Index", viewresult.ViewName);
        }

        [TestMethod]
        public async Task MachineView()
        {
            // Arrange
            List<Machine> machines = new List<Machine>()
            {
                new Machine
                {
                    Id = 1, Name = "Spanskrør", 
                    Warehouse = new Warehouse
                    {
                        City = "Vejle",
                        PostCode = 7100
                    }
                }, 
                new Machine
                {
                    Id = 2, Name = "Sømpistol",
                    Warehouse = new Warehouse
                    {
                        City = "Fredericia",
                        PostCode = 7000
                    }
                }
            };
            var WarehouseMock = new Mock<IHttpClientHandler<Warehouse>>();
            WarehousesController warehouseController = new WarehousesController(WarehouseMock.Object);
            var MachineMock = new Mock<IHttpClientHandler<Machine>>();
            MachineMock.Setup(m => m.GetAll("").Result).Returns(machines);
            MachinesController machinescontroller = new MachinesController(MachineMock.Object, warehouseController);
            // Act
            var actionResultTask = await machinescontroller.Index(new MachineQueryParameters());
            var viewResult = actionResultTask as ViewResult;
            //var resultList = viewResult.Model as List<Machine>;
            // Assert
            //Assert.IsNotNull(viewResult);
            //Assert.IsNotNull(resultList);
            Assert.IsNotNull(viewResult.ViewData.Model);
            Assert.AreEqual("Index", viewResult.ViewName);
            //Assert.AreEqual("Sømpistol" , resultList[1].Name);
        }

        //[TestMethod]
        //public async Task MachineDelete()
        //{
        //    //string apiMock = FakeApiUrlHere;
        //    var machineMock = new Mock<MachinesController>();
        //    machineMock.Setup(repo => repo.Delete(2)).
        //   .ReturnsAsync(TestDelete());

        //    MachinesController machinecontroller = new MachinesController();

        //    var actionResultTask = await machinecontroller.Delete();

        //    //await actionResultTask.Wait();
        //    var viewresult = actionResultTask as ViewResult;
        //    Assert.IsNotNull(viewresult);
        //    Assert.AreEqual("Delete", viewresult.ViewName);
        //}
        private List<Machine> TestDelete()
        {
            var sessions = new List<Machine>();
            sessions.Add(new Machine()
            {
                Id = 5,
                Name = "Traktor",
                Type = "Maskine"
            });
            return sessions;


        }
    }
}
