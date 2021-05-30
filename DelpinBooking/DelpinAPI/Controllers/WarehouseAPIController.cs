using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DelpinAPI.Data;
using System.Linq;
using DelpinAPI.APIModels;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using DelpinAPI.Classes;

namespace DelpinAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseAPIController : ControllerBase
    {
        private readonly DelpinContext _context;
        public WarehouseAPIController(DelpinContext context)
        {
            _context = context;

        }

        // Der skal laves Pagernating
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllWarehouses([FromQuery]WarehouseQueryParameters queryParameters)
        {
            IQueryable<Warehouse> warehouses = _context.Warehouse;
            Console.WriteLine("________________" + queryParameters.City);
            warehouses = warehouses
                .AsNoTracking()
                .FilterItems(queryParameters)
                .SortBy(queryParameters)
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);

            return Ok(await warehouses.ToListAsync());
        }
        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllWarehouseCities()
        {
            var warehouses = await _context.Warehouse
                .AsNoTracking()
                .Select(w => w.City)
                .ToListAsync();

            return Ok(warehouses);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetWarehouse(int id)
        {

            var Warehouse = await _context.Warehouse
                .AsNoTracking()
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            return Ok(Warehouse);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(Warehouse warehouse)
        {
            _context.Add(warehouse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWarehouse", new { id = warehouse.Id }, warehouse);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> Update(Warehouse warehouse)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            try
            {
                _context.Update(warehouse);
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
            var Warehouse = await _context.Warehouse
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            _context.Warehouse.Remove(Warehouse);

            await _context.SaveChangesAsync();

            return Ok(Warehouse);

        }

        private Dictionary<string, string> DiagnoseConflict(EntityEntry entry)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            var warehouse = entry.Entity as Warehouse;

            if (warehouse == null)
            {
                throw new NotSupportedException();
            }

            var databaseEntry = _context.Warehouse.AsNoTracking().SingleOrDefault(m => m.Id == warehouse.Id);
            if (databaseEntry == null)
            {
                errors.Add("Deleted", "Varehuset er blevet slettet af en anden bruger");
            }

            if (databaseEntry.City != warehouse.City)
            {
                errors.Add("City", "Nuværende værdi: " + databaseEntry.City);
            }

            if (databaseEntry.PostCode != warehouse.PostCode)
            {
                errors.Add("PostCode", "Nuværende værdi: " + databaseEntry.PostCode);
            }

            return errors;
        }
    }

    public static class WarehouseFilters
    {
        public static IQueryable<Warehouse> FilterItems(this IQueryable<Warehouse> warehouses, WarehouseQueryParameters queryParameters)
        {

            //Filtering Items, specifically Warehouse by City.
            if (!string.IsNullOrEmpty(queryParameters.City))
            {
                warehouses = warehouses.Where(
                    w => w.City == queryParameters.City);
            }

            if(queryParameters.PostCode != 0)
            {
                warehouses = warehouses.Where(
                    w => w.PostCode == queryParameters.PostCode);
            }

            return warehouses;
        }

        public static IQueryable<Warehouse> SortBy(this IQueryable<Warehouse> warehouses, WarehouseQueryParameters queryParameters)
        {
            if (string.IsNullOrEmpty(queryParameters.SortBy))
            {
                queryParameters.SortBy = "Alphabetic";
            }

            if (queryParameters.SortBy == "Alphabetic")
            {
                warehouses = warehouses.OrderByDescending(w => w.City);
            }
            

            return warehouses;
        }
    }
}
