using DelpinBooking.Classes;
using DelpinBooking.Controllers.Handler;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DelpinBooking.Controllers
{
    [Authorize]
    public class MachinesController : Controller
    {
        private HttpClientHandlerMachine _httpClientHandler;
        private WarehousesController _warehouseController;

        private string ApiUrl = "https://localhost:5001/api/MachineAPI/";


        public MachinesController(HttpClientHandlerMachine httpClientHandler, WarehousesController warehouseController)
        {
            _httpClientHandler = httpClientHandler;
            _warehouseController = warehouseController;
        }

        

        // GET: Machines
        public async Task<IActionResult> Index([Bind("Page,Size")] MachineQueryParameters queryParameters)
        {
            string queryString = "page=" + queryParameters.Page +
                                 "&size=" + queryParameters.Size;

            ViewBag.QueryParameters = queryParameters;

            List<Machine> Machines = await _httpClientHandler.GetAll(queryString);

            return View("Index", Machines);
        }

        public async Task<IActionResult> ChooseMachines([Bind("WarehouseCity")] MachineQueryParameters queryParameters)
        {
            string queryString = "page=" + queryParameters.Page +
                                 "&size=" + queryParameters.Size+
                                  "&available=" + true;

            if (queryParameters.WarehouseCity != null)
            {
                queryString += "warehousecity=" + queryParameters.WarehouseCity;
            }

            ViewBag.QueryParameters = queryParameters;

            List<Machine> machines = await _httpClientHandler.GetAll(queryString);
            
            
            List<string> WarehouseCities = await _warehouseController.GetAllWarehouseCities();
            ViewBag.WarehouseCities = WarehouseCities;


            return View(machines);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            Machine machine = await _httpClientHandler.Get(id);


            ShoppingCartController shoppingCartController = new ShoppingCartController {ControllerContext = ControllerContext };
            shoppingCartController.Add(machine);

            return RedirectToAction("ChooseMachines");
        }

        // GET: Machines/Details/5
        public async Task<IActionResult> Details(int id)
        {

            Machine machine = await _httpClientHandler.Get(id);


            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        // GET: Machines/Create
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create()
        {
           
            List<Warehouse> warehouses = await _warehouseController.GetAllWarehouses();
            ViewBag.Warehouses = warehouses;

            return View(new Machine());
        }

        // POST: Machines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type")] Machine machine, int warehouseId)
        {   
            if (ModelState.IsValid)
            {
             
                machine.Warehouse = await _warehouseController.GetWarehouse(warehouseId);
                Machine machineCreate = await _httpClientHandler.Create(machine);

                if (machineCreate != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(machine);
        }

        // GET: Machines/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int id)
        {
            

            var machine = await _httpClientHandler.Get(id);
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        // POST: Machines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,RowVersion")] Machine machine)
        {
            if (id != machine.Id)
            {
                return NotFound();
            }

            Machine machineToUpdate = await _httpClientHandler.Get(id);


            if (machineToUpdate == null)
            {
                Machine deletedMachine = new Machine();
                ModelState.AddModelError(string.Empty,
                    "Kan ikke gemme ændringerne. Maskinen blev slettet af en anden bruger.");

                return View(deletedMachine);
            }

            if (ModelState.IsValid)
            {


                Dictionary<string, string> errors = await _httpClientHandler.Update(machine);

                foreach (string e in errors.Keys)
                {
                    ModelState.AddModelError(e, errors[e]);
                }

                if (errors.Count == 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }

            return View(machine);

        }

        // GET: Machines/Delete/5
        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            Machine machine = await _httpClientHandler.Get(id);

            if (machine == null)
            {
                return NotFound();
            }

            return View("Delete", machine);
        }

        // POST: Machines/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Machine machineDelete = await _httpClientHandler.Delete(id);

            if (machineDelete != null)
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }


        private bool MachineExists(int id)
        {
            Machine machine = _httpClientHandler.Get(id).Result;
            return machine != null;
        }


    }
}
