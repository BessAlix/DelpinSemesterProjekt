using DelpinAPI.APIModels;
using DelpinAPI.Classes;
using DelpinAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetAllMachines()
        {
            var machines = await _context.Machine
                .AsNoTracking()
                .Include(p => p.Warehouse)
                .ToListAsync();

            return Ok(machines);
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAvailableMachines([FromQuery] MachineQueryParameters queryParameters)
        {
            IQueryable<Machine> machines = _context.Machine;

            //Filtering Items, specifically Warehouse by City.
            if (queryParameters.Warehouse != null)

            {
                machines = machines.Where(
                m => m.Warehouse.City == queryParameters.Warehouse.City);
            }
           

            //Searching items, specifically Name and Type.
            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                machines = machines.Where (
                m => m.Name.ToLower().Contains(queryParameters.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                machines = machines.Where(
                m => m.Type.ToLower().Contains(queryParameters.Type.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                if(typeof(Machine).GetProperty(queryParameters.SortBy) != null)
                {
                    machines = machines.OrderBy(m => m.Name);
                }
            }

            machines = machines
                .AsNoTracking()
                .Include(p => p.Warehouse)
                .Include(p => p.Booking)
                .Where(m => m.Booking == null)
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);

            
            return Ok(await machines.ToListAsync());
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetMachine(int id)
        {
            var machines = await _context.Machine
                .AsNoTracking()
                .Include(p => p.Warehouse)
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
                .Include(p => p.Booking)
                .Where(m => m.Booking == null)
                .Where(m => m.Name == name)
                .FirstOrDefaultAsync();
            return Ok(machines);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create(Machine machine)
        {
            _context.Add(machine);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetMachine", new { id = machine.Id }, machine);
        }

    }
}
