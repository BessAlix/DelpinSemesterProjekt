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
        [Authorize(Roles = "Admin, Employee")]
        [Route("[controller]/[action]")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Booking> Bookings;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ApiUrl + "GetAllBookings"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Bookings = JsonConvert.DeserializeObject<List<Booking>>(apiResponse); 
                    
                }
            }
            return View(Bookings);
        }
        // GET: Bookings for a customer 
        [Route("[controller]/[action]")]
        [HttpGet]
        public async Task<IActionResult> BookingsForCustomer(string id)
        {
            List<Booking> Bookings;
            var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(ApiUrl + "GetBookingsForCustomer/" + UserID))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Bookings = JsonConvert.DeserializeObject<List<Booking>>(
                        apiResponse); // substring to remove array brackets from response

                }
            }
            return View("Index", Bookings);
        }


        // GET: Bookings/Details/5
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Details(int? id)
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

            return View(booking);
        }

        // GET: Bookings/Create
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CreateAsync()
        {
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
                       SoftDeleted = false
                    };
                }
            }
              
            return View(booking);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PickUpDate,ReturnDate,Customer")] [FromForm] Booking booking)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var postTask = httpClient.PostAsJsonAsync<Booking>(ApiUrl + "Create", booking);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            return RedirectToAction(nameof(BookingsForCustomer));
                        }
                    }
                }
            }

            return View(booking);
        }


        [Authorize(Roles = "Admin, Employee")]
        // GET: Bookings/Edit/5
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
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
        [Route("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PickUpDate,ReturnDate,RentType,DepartmentStore,PricePrDay,CustomerID,PhoneNumber,CustomerName,CompanyName,Address,City")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            return View(booking);
        }

        // GET: Bookings/Delete/5
        [HttpGet]
        [Route("[controller]/[action]")]
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
        [Route("[controller]/[action]")]
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

                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(BookingsForCustomer));
                }
            }
        }

        // Opens new window with Customer information in Bookings
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

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.Id == id);
        }
    }
}
