using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DelpinBooking.Data;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace DelpinBooking.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class BookingsController : Controller
    {
        private readonly DelpinBookingContext _context;
        private readonly string ApiUrl = "https://localhost:5001/api/BookingAPI/";
        private readonly string UserUrl = "https://localhost:44379/ApplicationUsers/";

        public BookingsController(DelpinBookingContext context)
        {
            _context = context; 
        }

        // GET: Bookings
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Booking> Bookings;
            using (var httpClient = new HttpClient())
            {
                string method = "";
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    method = "GetAllBookings/";
                }

                else
                {
                    var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    method = "GetBookingsForCustomer/" + UserID;
                }

                using (var response = await httpClient.GetAsync(ApiUrl + method))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Bookings = JsonConvert.DeserializeObject<List<Booking>>(apiResponse);
                }
            }

            return View(Bookings);
        }

        // GET: Bookings/Details/5
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Booking booking;
            using (var httpClient = new HttpClient())
            {
                string method = "GetBooking/";
                using (var response = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    booking = JsonConvert.DeserializeObject<Booking>(apiResponse);
                }
            }

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        [HttpGet]
        [Route("[action]/{machinesstring}")]
        public async Task<IActionResult> CreateBooking(string machinesstring)
        {
            List<Machine> machines = JsonConvert.DeserializeObject<List<Machine>>(machinesstring);
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
                List<Machine> Machines = JsonConvert.DeserializeObject<List<Machine>>(machinesstring);
                using (var httpClient = new HttpClient()) 
                {
                    booking.Machines = Machines;

                    string method = "Create/";
                    using (var response = await httpClient.PostAsJsonAsync<Booking>(ApiUrl + method, booking))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            ShoppingCartController shoppingCartController = new ShoppingCartController { ControllerContext = ControllerContext };
                            shoppingCartController.Clear();
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            
            return View(booking);
        }


        // GET: Bookings/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking booking;
            using (var httpClient = new HttpClient())
            {
                string method = "GetBooking/";
                using (var response = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    booking = JsonConvert.DeserializeObject<Booking>(apiResponse);                   
                }
            }
           
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
            
            Booking bookingToUpdate;
            using (var httpClient = new HttpClient())
            {
                string method = "GetBooking/";
                using (var response = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    bookingToUpdate = JsonConvert.DeserializeObject<Booking>(apiResponse);
                }
            }

            if (bookingToUpdate == null)
            {
                Booking deletedBooking = new Booking();
                ModelState.AddModelError(string.Empty,
                    "Kan ikke gemme ændringerne. Bookingen blev slettet af en anden bruger.");

                return View(deletedBooking);
            }

            if (ModelState.IsValid)
            {                
                using (var httpClient = new HttpClient())
                {                 
                    Dictionary<string, string> errors;

                    string method = "Update/";
                    using (var response = await httpClient.PutAsJsonAsync<Booking>(ApiUrl + method, booking))
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

            return View(booking);
        }

        // GET: Bookings/Delete/5
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking booking;
            using (var httpClient = new HttpClient())
            {
                string method = "GetBooking/";
                using (var response = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    booking = JsonConvert.DeserializeObject<Booking>(apiResponse);
                }
            }

            if (booking == null)
            {
                return NotFound();
            }

            var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (User.IsInRole("Admin") || UserID == booking.Customer)
            {
                return View(booking);
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
                using (var httpClient = new HttpClient())
                {
                    string method = "Delete/";
                    using (var response = await httpClient.PostAsJsonAsync<Booking>(ApiUrl + method, booking))
                    {
                        return RedirectToAction(nameof(Index));
                    }     
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
