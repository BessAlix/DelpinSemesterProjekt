using System;
using System.Linq;
using DelpinAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DelpinAPI.APIModels;

namespace DelpinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingAPIController : ControllerBase
    {
        private readonly DelpinContext _context;
        public BookingAPIController(DelpinContext context)
        {
            _context = context;
          
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _context.Booking
                .AsNoTracking()
                .Include(p => p.Machine)
                .ToListAsync();
            return Ok(bookings);
        }

        [HttpGet]
        [Route("[action]/{customerId}")]
        public async Task<IActionResult> GetBookingsForCustomer(string customerId)
        {
            var bookings = await _context.Booking
                .AsNoTracking()
                .Where(b => b.Customer == customerId)
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpGet]
        [Route("[action]/{bookingId}")]
        public async Task<IActionResult> GetBooking(int bookingId)
        {
            var booking = await _context.Booking
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            return Ok(booking);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(Booking booking)
        {
            _context.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }


        [HttpPut]
        [Route("[action]/{bookingId}")]
        public async Task<IActionResult> Delete(int bookingId)
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(b => b.Id == bookingId);
            booking.SoftDeleted = true;
            await _context.SaveChangesAsync();
            Console.WriteLine("XXXXXXXXXXXXXX" + bookingId);
            return Ok(booking);
        }

        
    }
}
