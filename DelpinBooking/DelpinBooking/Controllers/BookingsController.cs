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
using System.Security.Claims;
using Newtonsoft.Json;
using DelpinBooking.Migrations;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using Machine = DelpinBooking.Models.Machine;

namespace DelpinBooking.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class BookingsController : Controller
    {

        private readonly DelpinBookingContext _context;
        private readonly string ApiUrl = "https://localhost:5001/api/BookingAPI/";


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
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    using (var response = await httpClient.GetAsync(ApiUrl + "GetAllBookings"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Bookings = JsonConvert.DeserializeObject<List<Booking>>(apiResponse);
                    }
                }
                else
                {
                    var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    using (var response = await httpClient.GetAsync(ApiUrl + "GetBookingsForCustomer/" + UserID))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        Bookings = JsonConvert.DeserializeObject<List<Booking>>(apiResponse);
                    }
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
                using (var response = await httpClient.GetAsync(ApiUrl + "GetBooking/" + id))
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
                using (var response = await httpClient.GetAsync("https://localhost:44379/applicationusers/getuser/" + UserID))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(
                        apiResponse.Substring(1, apiResponse.Length - 2)); // substring to remove array brackets from response
                    
                    booking = new Booking
                    {
                       Customer = user.Id,
                       SoftDeleted = false,
                       Machines = machines
                       
                    };
                    
                }
            }

            
            return View(booking);
        }
        

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    var postTask = await httpClient.PostAsJsonAsync<Booking>(ApiUrl + "Create", booking);
                    
                    return RedirectToAction(nameof(Index));
                }
            }
            
            return View(booking);
        }

        [Authorize(Roles = "Admin, Employee")]
        // GET: Bookings/Edit/5
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
                using (var response = await httpClient.GetAsync(ApiUrl + "GetBooking/" + id))
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

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PickUpDate,ReturnDate,Customer")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var putTask = await httpClient.PutAsJsonAsync<Booking>(ApiUrl + "Update", booking);
                    putTask.EnsureSuccessStatusCode();
                }

                return RedirectToAction(nameof(Index));
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

            var booking = await _context.Booking
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
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
        public async Task<IActionResult> DeleteConfirmed(int bookingId)
        {
            using (var httpClient = new HttpClient())
            {
                var endPoint = $"Delete/";
                httpClient.BaseAddress = new Uri(ApiUrl);

                var jsonObject = JsonConvert.SerializeObject(bookingId);
                var stringContent = new StringContent(jsonObject.ToString(), System.Text.Encoding.UTF8, "application/json");
                var respone = await httpClient.PostAsync(endPoint, stringContent);
                respone.EnsureSuccessStatusCode();
    
                return RedirectToAction(nameof(Index));
            }
        }

        // Opens new window with Customer information in Bookings
        [HttpGet]
        [Route("[action]")]
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

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }
    }
}
