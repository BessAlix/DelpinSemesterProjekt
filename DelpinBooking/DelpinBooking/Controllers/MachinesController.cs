using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DelpinBooking.Data;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace DelpinBooking.Controllers
{
    [Authorize]
    public class MachinesController : Controller
    {
        private readonly DelpinBookingContext _context;
        private readonly string ApiUrl = "https://localhost:5001/api/MachineAPI/";

        public MachinesController(DelpinBookingContext context)
        {
            _context = context;
        }

        // GET: Machines
        public async Task<IActionResult> Index(string? queryParameters)
        {
            if (string.IsNullOrEmpty(queryParameters))
            {
                queryParameters = "size=10&page=1";
                
            }

            string [] queryParametersArr = queryParameters.Split('&');
            Dictionary<string, int> queryParametersDictionary = new Dictionary<string, int>();
            foreach (var s in queryParametersArr)
            {
                string[] arr = s.Split('=');
                queryParametersDictionary.Add(arr[0], int.Parse(arr[1]));
            }

            ViewBag.QueryParametersDictionary = queryParametersDictionary;

            List<Machine> Machines;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ApiUrl + "GetAllMachines?" + queryParameters))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Machines = JsonConvert.DeserializeObject<List<Machine>>(apiResponse);

                }
            }

            return View(Machines);
        }

        public async Task<IActionResult> ChooseMachines(string? queryParameters)
        {
            List<Machine> Machines;
            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(queryParameters))
                {
                    queryParameters = "warehousecity=" + queryParameters;
                }
                using (var response = await httpClient.GetAsync(ApiUrl + "GetAvailableMachines?" + queryParameters))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Machines = JsonConvert.DeserializeObject<List<Machine>>(apiResponse);
                }
            }

            Dictionary<string, int> valuePairs = new Dictionary<string, int>();
            foreach (Machine machine in Machines)
            {
                if (valuePairs.ContainsKey(machine.Name))
                {
                    valuePairs[machine.Name]++;
                }
                else
                {
                    valuePairs.Add(machine.Name, 1);
                }
            }

            return View(valuePairs);
        }

        public async Task<IActionResult> AddToCart(string name)
        {
            Machine machine;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ApiUrl + "GetMachineFromName/" + name))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    machine = JsonConvert.DeserializeObject<Machine>(apiResponse);

                }
            }
            return RedirectToAction("Add", "ShoppingCart", machine);
        }

        // GET: Machines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machine
                .FirstOrDefaultAsync(m => m.Id == id);
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        // GET: Machines/Create
        [Authorize(Roles ="Admin,Employee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Machines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Type")] Machine machine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(machine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(machine);
        }


        // GET: Machines/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machine.FindAsync(id);
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

            Machine machineToUpdate;
            using (var httpClient = new HttpClient())
            {
                string method = "GetMachine/";
                using (var response = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    machineToUpdate = JsonConvert.DeserializeObject<Machine>(apiResponse);
                }
            }

            if (machineToUpdate == null)
            {
                Machine deletedMachine = new Machine();
                ModelState.AddModelError(string.Empty,
                    "Kan ikke gemme ændringerne. Maskine blev slettet af en anden bruger.");

                return View(deletedMachine);
            }

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    Dictionary<string, string> errors;

                    string method = "Update/";
                    using (var response = await httpClient.PutAsJsonAsync<Machine>(ApiUrl + method, machine))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        errors = JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);
                    }

                    foreach (string m in errors.Keys)
                    {
                        
                        ModelState.AddModelError(m, errors[m]);
                    }
                    
                    if (errors.Count == 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return View(machine);
        }

        // GET: Machines/Delete/5
        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
          
            Machine machine;
            using (var httpClient = new HttpClient())
            {
                string method = "GetMachine/";
                using (var response = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    machine = JsonConvert.DeserializeObject<Machine>(apiResponse);
                }
            }

            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        // POST: Machines/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine(id + "Is this working to delete");
            using (var httpClient = new HttpClient())
            {
                string method = "Delete/";
                using (var response = await httpClient.DeleteAsync(ApiUrl + method + id))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        private bool MachineExists(int id)
        {
            return _context.Machine.Any(e => e.Id == id);
        }
    }
}
