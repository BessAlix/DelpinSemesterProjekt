using DelpinBooking.Classes;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DelpinBooking.Models.Interfaces;

namespace DelpinBooking.Controllers
{
    [Authorize]
    public class MachinesController : Controller
    {
        private IHttpClientHandler<Machine> _httpClientHandler;
        private WarehousesController _warehouseController;

        private string ApiUrl = "https://localhost:5001/api/MachineAPI/";


        public MachinesController(IHttpClientHandler<Machine> httpClientHandler, WarehousesController warehouseController)
        {
            _httpClientHandler = httpClientHandler;
            _warehouseController = warehouseController;
        }

        // GET: Machines
        [HttpGet]
        public async Task<IActionResult> Index([Bind("Page,Size,WarehouseCity")] MachineQueryParameters queryParameters)
        {
            string viewToReturn = "Index";

            string queryString = "page=" + queryParameters.Page +
                                 "&size=" + queryParameters.Size;

            if (queryParameters.WarehouseCity != null)
            {
                queryString += "&warehousecity=" + queryParameters.WarehouseCity;
            }

            if (!(User.IsInRole("Admin") || User.IsInRole("Employee")))
            {
                queryString += "&available=" + true;
                viewToReturn = "ChooseMachines";
            }


            List<string> WarehouseCities = await _warehouseController.GetAllWarehouseCities();
            ViewBag.WarehouseCities = WarehouseCities;
            ViewBag.QueryParameters = queryParameters;

            List<Machine> Machines = await _httpClientHandler.GetAll(queryString);


            return View(viewToReturn, Machines);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            Machine machine = await _httpClientHandler.Get(id);

            ShoppingCartController shoppingCartController = new ShoppingCartController { ControllerContext = ControllerContext };
            shoppingCartController.Add(machine);

            return RedirectToAction(nameof(Index));
        }

        // GET: Machines/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Machine machine = await _httpClientHandler.Get(id);

            if (machine == null)
            {
                return NotFound();
            }

            return View("Details", machine);
        }

        // GET: Machines/Create
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create()
        {
            List<Warehouse> warehouses = await _warehouseController.GetAllWarehouses();
            ViewBag.Warehouses = warehouses;

            return View("Create", new Machine());
        }

        // POST: Machines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
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

            // Something want wrong
            // Either model state was invalid or the api didn't return Ok status
            return RedirectToAction(nameof(Create));
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

            return View("Edit", machine);
        }

        // POST: Machines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,RowVersion")] Machine machine)
        {
            if (id != machine.Id)
            {
                return NotFound();
            }

            Machine machineToUpdate = await _httpClientHandler.Get(id);
            if (machineToUpdate == null)
            {
                ModelState.AddModelError(string.Empty,
                    "Kan ikke gemme ændringerne. Maskinen blev slettet af en anden bruger.");

                return View("Edit");
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

            return View("Edit", machine);

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
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Machine machineDelete = await _httpClientHandler.Delete(id);

            if (machineDelete != null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Machine wasn't deleted
            // Redirects the user to the Delete action so they can try again
            return RedirectToAction(nameof(Delete), id);
        }

        private bool MachineExists(int id)
        {
            Machine machine = _httpClientHandler.Get(id).Result;
            return machine != null;
        }
    }
}
