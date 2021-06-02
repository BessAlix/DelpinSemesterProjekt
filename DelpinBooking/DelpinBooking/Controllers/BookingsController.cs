using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DelpinBooking.Data;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Net.Http;
using DelpinBooking.Classes;
using System.Net.Http.Json;
using DelpinBooking.Controllers.Handler;
using DelpinBooking.Models.Interfaces;

namespace DelpinBooking.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class BookingsController : Controller
    {
        private readonly DelpinBookingContext _context;
        private readonly string UserUrl = "https://localhost:44379/ApplicationUsers/";
        private IHttpClientHandler<Booking> _httpClientHandler;

        public BookingsController(IHttpClientHandler<Booking> httpClientHandler)
        {
            _httpClientHandler = httpClientHandler; 
        }

        // GET: Bookings
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Index([Bind("Page,Size,SearchBy")]BookingQueryParameters queryParameters)
        {
            string queryString = "page=" + queryParameters.Page +
                                  "&size=" + queryParameters.Size;

            if (queryParameters.SearchBy != null)
            {
                queryString += "&searchby=" + queryParameters.SearchBy;
            }

            ViewBag.QueryParameters = queryParameters;

            List<Booking> Bookings;
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                Bookings = await _httpClientHandler.GetAll(queryString);
            }
            else
            {
                var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                queryString += "&customer=" + UserID;
                Bookings = await _httpClientHandler.GetAll(queryString);
            }

            return View("Index", Bookings);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> MachinesForBooking(string machinesString)
        {
            List<Machine> machines = JsonConvert.DeserializeObject<List<Machine>>(machinesString);

            return View(machines);
        }

        // GET: Bookings/Details/5
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Details(int id)
        {
            Booking booking = await _httpClientHandler.Get(id);
            

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CreateBooking(string machinesString)
        {
            List<Machine> machines = JsonConvert.DeserializeObject<List<Machine>>(machinesString);
            Booking booking;
            var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            using (var httpClient = new HttpClient())
            {
                string method = "GetUser/";
                using (var response = await httpClient.GetAsync(UserUrl + method + UserID))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(apiResponse);
                    
                    booking = new Booking
                    {
                       Customer = user.Id,
                       SoftDeleted = false,
                       Machines = machines                       
                    };                    
                }
            }
            
            return View("Create", booking);
        }
        

        // POST: Bookings/Create
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string machinesstring,[Bind("Id,PickUpDate,ReturnDate,Customer")] [FromForm] Booking booking)
        {
            if (ModelState.IsValid)
            {
                List<Machine> machines = JsonConvert.DeserializeObject<List<Machine>>(machinesstring);
                
                // Removes warehouse for machines to avoid tracking duplication on Id
                // (Doesn't remove the foreign key in the machines table)
                foreach (Machine m in machines)
                {
                    m.Warehouse = null;
                }
                booking.Machines = machines;
                Booking bookingResponse = await _httpClientHandler.Create(booking);

                if(bookingResponse != null)
                {
                    ShoppingCartController shoppingcart = new ShoppingCartController {ControllerContext = ControllerContext };
                    shoppingcart.Clear();
                    return RedirectToAction("Index");
                }

                
            }
            
            return View(booking);
        }


        // GET: Bookings/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit(int id)
        {
            Booking booking = await _httpClientHandler.Get(id);

            return View(booking);
        }

        // POST: Bookings/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PickUpDate,ReturnDate,Customer,RowVersion")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }
            
            Booking bookingToUpdate = await _httpClientHandler.Get(id);
            

            if (bookingToUpdate == null)
            {
                Booking deletedBooking = new Booking();
                ModelState.AddModelError(string.Empty,
                    "Kan ikke gemme ændringerne. Bookingen blev slettet af en anden bruger.");

                return View(deletedBooking);
            }

            if (ModelState.IsValid)
            {                
                
                                 
            Dictionary<string, string> errors = await _httpClientHandler.Update(booking);

            foreach (string e in errors.Keys)
            {   
                ModelState.AddModelError(e, errors[e]);
            }

            if (errors.Count == 0)
            {
               return RedirectToAction(nameof(Index));
            }
                
            }

            return View(booking);
        }

        // GET: Bookings/Delete/5
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Delete(int id)
        {
         
            Booking booking = await _httpClientHandler.Get(id);
            

            if (booking == null)
            {
                return NotFound();
            }

            var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (User.IsInRole("Admin") || UserID == booking.Customer)
            {
                return View("Delete", booking);
            }
            else
            {
                return View("NotAuthorized");
            }
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("Id,Customer")] Booking booking)
        {
            var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("Admin") || UserID == booking.Customer)
            {
                Booking bookingDelete = await _httpClientHandler.Delete(booking.Id);

                Console.WriteLine("MVC Booking ID" + bookingDelete.Id);
                if(bookingDelete != null)
                {
                 
                   return View("DeleteCompleted", bookingDelete);
                }
             
            }
            
            return View("NotAuthorized");
        }

        // Opens new window with Customer information in Bookings
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "GetUser/";
                using (var response = await httpClient.GetAsync(UserUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(apiResponse);
                    return View(user);
                }
            }
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }
    }
}
