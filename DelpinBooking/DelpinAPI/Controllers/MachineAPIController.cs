using DelpinAPI.APIModels;
using DelpinAPI.Classes;
using DelpinAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DelpinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineAPIController : ControllerBase
    {
        private readonly DelpinContext _context;
        public MachineAPIController(DelpinContext context)
        {
            _context = context;

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllMachines([FromQuery] MachineQueryParameters queryParameters)
        {
            IQueryable<Machine> machines = _context.Machine.AsNoTracking();

            machines = machines
                //.AsNoTracking()
                .Include(p => p.Warehouse)
                .AsNoTracking()
                .FilterItems(queryParameters)
                .SearchItems(queryParameters)
                .SortBy(queryParameters)
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);

            return Ok(await machines.ToListAsync());
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAvailableMachines([FromQuery] MachineQueryParameters queryParameters)
        {
            IQueryable<Machine> machines = _context.Machine.AsNoTracking();

            machines = machines
                //.AsNoTracking()
                .Include(p => p.Warehouse)
                .AsNoTracking()
                .Include(p => p.Booking)
                .AsNoTracking()
                .Where(m => m.Booking == null)
                .FilterItems(queryParameters)
                .SearchItems(queryParameters)
                .SortBy(queryParameters);

            return Ok(await machines.ToListAsync());
        }


        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetMachine(int id)
        {
            
            var machines = await _context.Machine
                .AsNoTracking()
                .Where(m => m.Id == id)
                .Include(p => p.Warehouse)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return Ok(machines);
        }

        [HttpGet]
        [Route("[action]/{name}")]
        public async Task<IActionResult> GetMachineFromName(string name)
        {
            var machines = await _context.Machine
                .AsNoTracking()
                .Include(p => p.Warehouse)
                .AsNoTracking()
                .Include(p => p.Booking)
                .AsNoTracking()
                .Where(m => m.Booking == null)
                .Where(m => m.Name == name)
                .FirstOrDefaultAsync();

            return Ok(machines);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(Machine machine)
        {
            _context.Update(machine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMachine", new { id = machine.Id }, machine);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> Update(Machine machine)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            try
            {
                _context.Update(machine);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                errors = DiagnoseConflict(entry);

            }

            return Ok(errors);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
            var machine = await _context.Machine
                .Include(p => p.Warehouse)
                .Include(b => b.Booking)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            _context.Machine.Remove(machine);
           
            await _context.SaveChangesAsync();
            
            return Ok(machine);
            
        }


        
        private Dictionary<string, string> DiagnoseConflict(EntityEntry entry)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            var machine = entry.Entity as Machine;

            if (machine == null)
            {
                throw new NotSupportedException();
            }

            var databaseEntry = _context.Machine.AsNoTracking().SingleOrDefault(m => m.Id == machine.Id);
            if (databaseEntry == null)
            {
                errors.Add("Deleted", "Maskine er blevet slettet af en anden bruger");
            }

            if (databaseEntry.Name != machine.Name)
            {
                errors.Add("Name", "Nuværende værdi: " + databaseEntry.Name);
            }

            if (databaseEntry.Type != machine.Type)
            {
                errors.Add("Type", "Nuværende værdi: " + databaseEntry.Type);
            }

            return errors;

        }
    }

    public static class MachineFilters
    {
        public static IQueryable<Machine> FilterItems(this IQueryable<Machine> machines, MachineQueryParameters queryParameters)
        {

            //Filtering Items, specifically Warehouse by City.
            if (!string.IsNullOrEmpty(queryParameters.WarehouseCity))
            {
                machines = machines.Where(
                    m => m.Warehouse.City == queryParameters.WarehouseCity);
            }

            if (queryParameters.Available)
            {
                machines = machines.Where(m => m.Booking == null);
            }

            return machines;
        }

        public static IQueryable<Machine> SearchItems(this IQueryable<Machine> machines, MachineQueryParameters queryParameters)
        {
            //Searching items, specifically Name and Type.
            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                machines = machines.Where(
                    m => m.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.Type))
            {
                machines = machines.Where(
                    m => m.Type.ToLower().Contains(queryParameters.Type.ToLower()));
            }

            return machines;
        }

        public static IQueryable<Machine> SortBy(this IQueryable<Machine> machines, MachineQueryParameters queryParameters)
        {
            // Sorts the item by Name
            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                if (typeof(Machine).GetProperty(queryParameters.SortBy) != null)
                {
                    machines = machines.OrderBy(m => m.Name);
                }
            }

            return machines;
        }
    }
}
