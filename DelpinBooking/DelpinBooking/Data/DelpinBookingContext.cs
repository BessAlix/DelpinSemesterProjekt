using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DelpinBooking.Models;

namespace DelpinBooking.Data
{
    public class DelpinBookingContext : DbContext
    {
        public DelpinBookingContext (DbContextOptions<DelpinBookingContext > options)
            : base(options)
        {
        }

        public DbSet<Booking> Booking { get; set; }
    }
}
