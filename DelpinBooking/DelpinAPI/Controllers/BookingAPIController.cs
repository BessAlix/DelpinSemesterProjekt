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
                .Include(p => p.Machines)
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
        public async Task<IActionResult> Create([FromBody]Booking booking)
        {    // We update within create
            //For each booking we write a foreign key 
            //Add method will make a new machine with a new ID
            //Update every machines foreign key to booking
            //Update checks if the machines ID already exists, if not, then create a new.
            _context.Update(booking);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }  

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> Update(Booking booking)
        {
            _context.Update(booking);
            await _context.SaveChangesAsync();

            return Ok(booking);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Delete([FromBody]int bookingId)
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(b => b.Id == bookingId);
            booking.SoftDeleted = true;
            await _context.SaveChangesAsync();

            return Ok(booking);
        }

        
    }
}
