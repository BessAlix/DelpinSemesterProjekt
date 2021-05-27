﻿using System;
using System.Linq;
using DelpinAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DelpinAPI.APIModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using DelpinAPI.Classes;

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
        public async Task<IActionResult> GetAllBookings([FromQuery] BookingQueryParameters queryParameters)
        {
            IQueryable<Booking> bookings = _context.Booking;

            bookings = FilterItems(queryParameters, bookings);
            bookings = SearchItems(queryParameters, bookings);
            bookings = SortBy(queryParameters, bookings);
           


            bookings = bookings
                .AsNoTracking()
                .Include(b => b.Machines);
            if(bookings.Count() > 0)
            {
                bookings = bookings
               .Skip(queryParameters.Size * (queryParameters.Page - 1))
               .Take(queryParameters.Size);
            }


            return Ok(await bookings.ToListAsync());
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

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Delete([FromBody]Booking booking)
        {
            var bookingToDelete = await _context.Booking.FirstOrDefaultAsync(b => b.Id == booking.Id);
            bookingToDelete.SoftDeleted = true;

            //Sql command to clear all foreign keys to the soft-deleted booking
            string sql = "UPDATE dbo.Machine SET BookingId = null" +
                          " WHERE BookingId = " + bookingToDelete.Id;

            _context.Database.ExecuteSqlRaw(sql);

            await _context.SaveChangesAsync();

            return Ok(bookingToDelete);
        }
        private IQueryable<Booking> FilterItems(BookingQueryParameters queryParameters, IQueryable<Booking> bookings)
        {

            //Filtering Items, specifically Warehouse by City.
            if (!string.IsNullOrEmpty(queryParameters.Customer))
            {
                bookings = bookings.Where(
                    b => b.Customer == queryParameters.Customer);
            }

            return bookings;
        }

        private IQueryable<Booking> SearchItems(BookingQueryParameters queryParameters, IQueryable<Booking> bookings)
        {
            //Searching items, specifically Name and Type.
            if (!string.IsNullOrEmpty(queryParameters.SearchBy))
            {
                IQueryable<Booking> bookingsCustomer = bookings.Where(
                m => m.Customer.Contains(queryParameters.SearchBy));
                    
               IQueryable<Booking> bookingsId = bookings.Where(
               m => m.Id.ToString().Contains(queryParameters.SearchBy));

                bookings= bookingsCustomer.Union(bookingsId);
            }   
            return bookings;
        }

        private IQueryable<Booking> SortBy(BookingQueryParameters queryParameters, IQueryable<Booking> bookings)
        {
            if (string.IsNullOrEmpty(queryParameters.SortBy))
            {
                queryParameters.SortBy = "Latest";
            }
            if(queryParameters.SortBy == "Latest")
            {
                bookings = bookings.OrderByDescending(b => b.Id);
            }
            
                if (queryParameters.SortBy == "ReturnDate")
                {
                    bookings = bookings.OrderBy(b => b.ReturnDate);
                }
                if (queryParameters.SortBy == "PickUpDate")
                {
                    bookings = bookings.OrderBy(b => b.PickUpDate);
                }
                if (queryParameters.SortBy == "Customer")
                {
                    bookings = bookings.OrderBy(b => b.Customer);
                }
            
            return bookings;
        }
        private Dictionary<string, string> DiagnoseConflict(EntityEntry entry)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            var booking = entry.Entity as Booking;

            if (booking == null)
            {
                throw new NotSupportedException();
            }

            var databaseEntry = _context.Booking.AsNoTracking().SingleOrDefault(b => b.Id == booking.Id);
            if (databaseEntry == null)
            {
                errors.Add("Deleted", "bookingen er blevet slettet af en anden bruger");
            }

            if (databaseEntry.PickUpDate != booking.PickUpDate)
            {
                errors.Add("PickUpDate", "Nuværende værdi: " + databaseEntry.PickUpDate);
            }

            if (databaseEntry.ReturnDate != booking.ReturnDate)
            {
                errors.Add("ReturnDate", "Nuværende værdi: " + databaseEntry.ReturnDate);
            }

            return errors;
        }
    }
}
