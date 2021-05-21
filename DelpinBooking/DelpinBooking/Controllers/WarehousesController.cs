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
        [Route("[controller]/[action]")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Warehouse> Warehouses;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ApiUrl + "GetAllWarehouses"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Warehouses = JsonConvert.DeserializeObject<List<Warehouse>>(apiResponse);

                }
            }
            return View(Warehouses);
        }
        // GET: Warehouses for a customer 
        [Route("[controller]/[action]")]
        [HttpGet]
        public async Task<List<string>> GetAllWarehouses()
        {
            List<string> Warehouses;
            
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ApiUrl + "GetWarehouseCities/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Warehouses = JsonConvert.DeserializeObject<List<string>>(
                        apiResponse); // substring to remove array brackets from response

                }
            }
            return Warehouses;
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
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CreateAsync()
        {
            Warehouse Warehouse;
            var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44379/applicationusers/getuser/" + UserID))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(
                        apiResponse.Substring(1, apiResponse.Length - 2)); // substring to remove array brackets from response
                    Warehouse = new Warehouse
                    {
                        
                    };
                    //warehouse = new Warehouse
                    //{

                    //}
                }
            }

            return View(Warehouse);
        }

        // POST: Warehouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PickUpDate,ReturnDate,Customer")][FromForm] Warehouse Warehouse)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var postTask = httpClient.PostAsJsonAsync<Warehouse>(ApiUrl + "Create", Warehouse);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        if (User.IsInRole("Admin") || User.IsInRole("Employee"))
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
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Warehouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PickUpDate,ReturnDate,RentType,DepartmentStore,PricePrDay,CustomerID,PhoneNumber,CustomerName,CompanyName,Address,City")] Warehouse Warehouse)
        {
            if (id != Warehouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Warehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(Warehouse.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Warehouse);
        }

        // GET: Warehouses/Delete/5
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Warehouse = await _context.Warehouse
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Warehouse == null)
            {
                return NotFound();
            }

            var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
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
        [HttpPost, ActionName("Delete")]
        [Route("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int WarehouseId)
        {
            using (var httpClient = new HttpClient())
            {
                var endPoint = $"Delete/";
                httpClient.BaseAddress = new Uri(ApiUrl);

                var jsonObject = JsonConvert.SerializeObject(WarehouseId);
                var stringContent = new StringContent(jsonObject.ToString(), System.Text.Encoding.UTF8, "application/json");
                var respone = await httpClient.PostAsync(endPoint, stringContent);
                respone.EnsureSuccessStatusCode();

                
                    return RedirectToAction(nameof(Index));
               
            }
        }

        // Opens new window with Customer information in Warehouses
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response =
                    await httpClient.GetAsync("https://localhost:44379/applicationusers/getuser/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(
                        apiResponse.Substring(1,
                            apiResponse.Length - 2)); // substring to remove array brackets from response
                    return View(user);
                }
            }

        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouse.Any(e => e.Id == id);
        }
    }
}
