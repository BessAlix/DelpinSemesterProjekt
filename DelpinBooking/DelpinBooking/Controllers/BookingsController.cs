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

namespace DelpinBooking.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class BookingsController : Controller
    {

        private readonly DelpinBookingContext _context;

        public BookingsController(DelpinBookingContext context)
        {
            _context = context;
        }

        // GET: Bookings
        [Route("[controller]/[action]")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var Bookings = await _context.Booking.ToListAsync();
            return View(Bookings);
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
            Booking model;
            var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44379/applicationusers/getuser/" + UserID))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(
                        apiResponse.Substring(1, apiResponse.Length - 2)); // substring to remove array brackets from response
                    model = new Booking
                    {
                       Customer = user.Id
                        
                    };
                }
            }
              
            return View(model);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PickUpDate,ReturnDate,RentType,DepartmentStore")] [FromForm] Booking booking)
        {
            if (ModelState.IsValid)
            {
                var UserID = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                using (var httpClient = new HttpClient())
                {
                    using (var response =
                        await httpClient.GetAsync("https://localhost:44379/applicationusers/getuser/" + UserID))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(
                            apiResponse.Substring(1, apiResponse.Length - 2));
                        booking.Customer = user.Id;
                        _context.Add(booking);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }

                
            }

            return View(booking);
            
        }


        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Employee")]
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

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("[controller]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int bookingId)
        {
            var booking = await _context.Booking.FindAsync(bookingId);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> GetCustomer(string customerId)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response =
                    await httpClient.GetAsync("https://localhost:44379/applicationusers/getuser/" + customerId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("*************" + apiResponse);
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
