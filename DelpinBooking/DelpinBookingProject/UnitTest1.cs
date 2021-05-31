using DelpinBooking.Classes;
using DelpinBooking.Controllers;
using DelpinBooking.Data;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DelpinBookingProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void HomeView()
        {
            var HomeRepository = new Mock<ILogger<HomeController>>();
            HomeController homecontroller = new HomeController(HomeRepository.Object);
            //var actionResultTask = homecontroller.Index();
            //await actionResultTask.Wait();
            var viewresult = homecontroller.Index() as ViewResult;
            Assert.IsNotNull(viewresult);
            Assert.AreEqual("Index", viewresult.ViewName);
        }

        [TestMethod]
        public async Task MachineView()
        {
            //var MachineRepository = new Mock<ILogger<HomeController>>();
            MachinesController machinecontroller = new MachinesController();
            var actionResultTask = await machinecontroller.Index(new MachineQueryParameters());
            //await actionResultTask.Wait();
            var viewresult = actionResultTask as ViewResult;
            Assert.IsNotNull(viewresult);
            Assert.AreEqual("Index", viewresult.ViewName);
        }

        [TestMethod]
        public async Task MachineDelete()
        {
            //string apiMock = FakeApiUrlHere;
            var machineMock = new Mock<MachinesController>();
            machineMock.Setup(repo => repo.Delete(2)).
           .ReturnsAsync(TestDelete());

            MachinesController machinecontroller = new MachinesController();

            var actionResultTask = await machinecontroller.Delete();

            //await actionResultTask.Wait();
            var viewresult = actionResultTask as ViewResult;
            Assert.IsNotNull(viewresult);
            Assert.AreEqual("Delete", viewresult.ViewName);
        }
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
