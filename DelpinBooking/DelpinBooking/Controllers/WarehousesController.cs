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
using DelpinBooking.Classes;
using DelpinBooking.Controllers.Handler;
using DelpinBooking.Models.Interfaces;

namespace DelpinBooking.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class WarehousesController : Controller
    {

        private IHttpClientHandler<Warehouse> _httpClientHandler;
        

        public WarehousesController(IHttpClientHandler<Warehouse> httpClientHandler)
        {
            _httpClientHandler = httpClientHandler;
        }

        // GET: Warehouses
        [Authorize(Roles = "Admin, Employee")]
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Index([Bind("Page,Size,City")] WarehouseQueryParameters queryParameters)
        {
            string queryString = "page=" + queryParameters.Page +
                                  "&size=" + queryParameters.Size;

            if (!string.IsNullOrEmpty(queryParameters.City))
            {
                int postCode = 0;
                if (int.TryParse(queryParameters.City, out postCode))
                {
                    queryParameters.PostCode = postCode;
                }
                else
                {
                    queryString += "&city=" + queryParameters.City;
                }

            }
            if (queryParameters.PostCode != 0)
            {
                queryString += "&postcode=" + queryParameters.PostCode;
            }
            if (queryParameters.SortBy != null)
            {
                queryString += "&sortby=" + queryParameters.SortBy;
            }

            ViewBag.QueryParameters = queryParameters;

            List<Warehouse> Warehouses = await _httpClientHandler.GetAll(queryString);


            return View("Index", Warehouses);
        }

        // GET: Warehouses for a customer 
        [Route("[action]")]
        [HttpGet]
        public async Task<List<string>> GetAllWarehouseCities()
        {
            List<string> warehouseCities = new List<string>();
            List<Warehouse> warehouseList = await _httpClientHandler.GetAll("");
            foreach (Warehouse w in warehouseList)
            {
                warehouseCities.Add(w.City);
            }
            return warehouseCities;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<List<Warehouse>> GetAllWarehouses()
        {
            
           List <Warehouse> warehouses = await _httpClientHandler.GetAll("");
      
            return warehouses;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<Warehouse> GetWarehouse(int id)
        {
            Warehouse warehouse = await _httpClientHandler.Get(id);
            

            return warehouse;
        }

        // GET: Warehouses/Details/5
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Warehouse Warehouse = await _httpClientHandler.Get(id);
           

            if (Warehouse == null)
            {
                return NotFound();
            }
            return View(Warehouse);
        }

        // GET: Warehouses/Create
        [Authorize(Roles = "Admin,Employee")]
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
                Warehouse warehouseCreate = await _httpClientHandler.Create(Warehouse);
            }
            return View(Warehouse);
        }


        [Authorize(Roles = "Admin, Employee")]
        // GET: Warehouses/Edit/5
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit(int id)
        {
            Warehouse Warehouse = await _httpClientHandler.Get(id);
           
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
            Warehouse warehouseToUpdate = await _httpClientHandler.Get(id);

            if (warehouseToUpdate == null)
            {
                Warehouse deletedWarehouse = new Warehouse();
                ModelState.AddModelError(string.Empty,
                    "Kan ikke gemme ændringerne. Varehuset er blevet slettet af en anden bruger.");

                return View(deletedWarehouse);
            }

            if (ModelState.IsValid)
            {
                    Dictionary<string, string> errors = await _httpClientHandler.Update(Warehouse);

                    foreach (string b in errors.Keys)
                    {
                        ModelState.AddModelError(b, errors[b]);
                    }

                    if (errors.Count == 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            return View(Warehouse);
        }

          
        

        // GET: Warehouses/Delete/5
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Delete(int id)
        {
            Warehouse Warehouse = await _httpClientHandler.Get(id);


            if (Warehouse == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin"))
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
            Warehouse warehouseDelete = await _httpClientHandler.Delete(id);

            if (warehouseDelete != null)
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }
    }
}
