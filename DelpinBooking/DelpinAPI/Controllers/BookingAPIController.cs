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
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _context.Booking
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

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

    }
}
