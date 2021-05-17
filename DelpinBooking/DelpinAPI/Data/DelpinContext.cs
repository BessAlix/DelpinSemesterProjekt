using DelpinAPI.APIModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinAPI.Data
{
    public class DelpinContext : DbContext
    {
        
        public DelpinContext(DbContextOptions<DelpinContext> options)
           : base(options)
        {
        }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Machine> Machine { get; set; }
        public DbSet <Warehouse> Warehouse { get; set; }
    }
  
}
