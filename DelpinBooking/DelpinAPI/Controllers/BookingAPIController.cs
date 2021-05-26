using System;
using System.Linq;
using DelpinAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DelpinAPI.APIModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

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
                //.Include(p => p.Machines)
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
        {   //We update within create
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
            Dictionary<string, string> errors = new Dictionary<string, string>();
            try 
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                errors = DiagnoseConflict(entry);

            }
            
            return Ok(errors);
        }
        private Dictionary<string, string> DiagnoseConflict(EntityEntry entry)
        { 
            Dictionary<string, string> errors = new Dictionary<string, string>();
            var booking = entry.Entity as Booking;
            
            if(booking == null)
            {
                throw new NotSupportedException();
            }

            var databaseEntry = _context.Booking.AsNoTracking().SingleOrDefault(b => b.Id == booking.Id);
            if(databaseEntry == null)
            {
                errors.Add("Deleted", "bookingen er blevet slettet af en anden bruger");
                
            }
            

            if(databaseEntry.PickUpDate != booking.PickUpDate)
            {
                errors.Add("PickUpDate", "Nuværende værdi: " + databaseEntry.PickUpDate);
            }
            if (databaseEntry.ReturnDate != booking.ReturnDate)
            {
                errors.Add("ReturnDate", "Nuværende værdi: " + databaseEntry.ReturnDate);
            }
            return errors;
        }   


        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Delete([FromBody]int bookingId)
        {
            var booking = await _context.Booking.FirstOrDefaultAsync(b => b.Id == bookingId);
            booking.SoftDeleted = true;

            //Sql command to clear all foreign keys to the soft-deleted booking
            string sql = "UPDATE dbo.Machine SET BookingId = null" +
                          " WHERE BookingId = " + bookingId;

            _context.Database.ExecuteSqlRaw(sql);
            await _context.SaveChangesAsync();

            return Ok(booking);
        }
    }
}
