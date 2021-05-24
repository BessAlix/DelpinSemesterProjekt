using DelpinAPI.APIModels;
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
        public async Task<IActionResult> GetAvailableMachines()
        {
            var machines = await _context.Machine
                .AsNoTracking()
                .Include(p => p.Warehouse)
                .Include(p => p.Booking)
                .Where(m => m.Booking == null)
                .ToListAsync();

            return Ok(machines);
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
