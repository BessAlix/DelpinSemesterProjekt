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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Type")] Machine machine)
        {
            if (id != machine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(machine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MachineExists(machine.Id))
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
            return View(machine);
        }

        // GET: Machines/Delete/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Machines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var machine = await _context.Machine.FindAsync(id);
            _context.Machine.Remove(machine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MachineExists(int id)
        {
            return _context.Machine.Any(e => e.Id == id);
        }
    }
}
