using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using DelpinBooking.Models;
using DelpinBooking.Data;

namespace DelpinBooking.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class WarehousesController : Controller
    {

        private readonly DelpinBookingContext _context;
        private readonly string ApiUrl = "https://localhost:5001/api/WarehouseAPI/";

        public WarehousesController(DelpinBookingContext context)
        {
            _context = context;
        }

        // GET: Warehouses
        [Authorize(Roles = "Admin, Employee")]
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Warehouse> Warehouses;
            using (var httpClient = new HttpClient())
            {
                string method = "GetAllWarehouses/";
                using (var response = await httpClient.GetAsync(ApiUrl + method))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Warehouses = JsonConvert.DeserializeObject<List<Warehouse>>(apiResponse);

                }
            }
            return View(Warehouses);
        }
        // GET: Warehouses for a customer 
        [Route("[action]")]
        [HttpGet]
        public async Task<List<string>> GetAllWarehouseCities()
        {
            List<string> warehouseCities;            
            using (var httpClient = new HttpClient())
            {
                string method = "GetAllWarehouseCities/";
                using (var response = await httpClient.GetAsync(ApiUrl + method))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    warehouseCities = JsonConvert.DeserializeObject<List<string>>(apiResponse);
                }
            }

            return warehouseCities;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<List<Warehouse>> GetAllWarehouses()
        {
            List<Warehouse> warehouses;
            using (var httpClient = new HttpClient())
            {
                string method = "GetAllWarehouses/";
                using (var response = await httpClient.GetAsync(ApiUrl + method))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    warehouses = JsonConvert.DeserializeObject<List<Warehouse>>(apiResponse);
                }
            }

            return warehouses;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Warehouse> GetWarehouse(int id)
        {
            Warehouse warehouse;
            using (HttpClient httpClient = new HttpClient())
            {
                string method = "GetWarehouse/";
                using (var resposne = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await resposne.Content.ReadAsStringAsync();
                    warehouse = JsonConvert.DeserializeObject<Warehouse>(apiResponse);
                }
            }

            return warehouse;
        }

        // GET: Warehouses/Details/5
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Warehouse Warehouse;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ApiUrl + "GetWarehouse/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Warehouse = JsonConvert.DeserializeObject<Warehouse>(apiResponse);

                }
            }

            if (Warehouse == null)
            {
                return NotFound();
            }
            return View(Warehouse);
        }

        // GET: Warehouses/Create
        [Authorize(Roles ="Admin,Employee")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Warehouse());
        }

        // POST: Warehouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWarehouse([Bind("Id,City,PostCode")][FromForm] Warehouse Warehouse)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    string method = "Create/";
                    using (var response = await httpClient.PostAsJsonAsync<Warehouse>(ApiUrl + method, Warehouse))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            return View(Warehouse);
        }


        [Authorize(Roles = "Admin, Employee")]
        // GET: Warehouses/Edit/5
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Warehouse Warehouse;
            using (var httpClient = new HttpClient())
            {
                string method = "GetWarehouse/";
                using (var response = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Warehouse = JsonConvert.DeserializeObject<Warehouse>(apiResponse);

                }
            }
            if (Warehouse == null)
            {
                return NotFound();
            }
            return View(Warehouse);
        }

        // POST: Warehouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,City,PostCode,RowVersion")] Warehouse Warehouse)
        {
            if (id != Warehouse.Id)
            {
                return NotFound();
            }

            Warehouse warehouseToUpdate;
            using (var httpClient = new HttpClient())
            {
                string method = "GetWarehouse/";
                using (var response = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    warehouseToUpdate = JsonConvert.DeserializeObject<Warehouse>(apiResponse);
                }
            }

            if (warehouseToUpdate == null)
            {
                Warehouse deletedWarehouse = new Warehouse();
                ModelState.AddModelError(string.Empty,
                    "Kan ikke gemme ændringerne. Varehuset er blevet slettet af en anden bruger.");

                return View(deletedWarehouse);
            }

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    Dictionary<string, string> errors;

                    string method = "Update/";
                    using (var response = await httpClient.PutAsJsonAsync<Warehouse>(ApiUrl + method, Warehouse))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        errors = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);
                    }

                    foreach (string b in errors.Keys)
                    {
                        ModelState.AddModelError(b, errors[b]);
                    }

                    if (errors.Count == 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return View(Warehouse);
        }

        // GET: Warehouses/Delete/5
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Warehouse Warehouse;
            using (var httpClient = new HttpClient())
            {
                string method = "GetWarehouse/";
                using (var response = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Warehouse = JsonConvert.DeserializeObject<Warehouse>(apiResponse);
                }
            }

            if (Warehouse == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin") )
            {
                return View(Warehouse);
            }
            else
            {
                return View("NotAuthorized");
            }
        }

        // POST: Warehouses/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "Delete/";
                using (var response = await httpClient.DeleteAsync(ApiUrl + method + id))
                {
                   
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        private bool WarehouseExists(int id)
        {
            return _context.Warehouse.Any(e => e.Id == id);
        }
    }
}
