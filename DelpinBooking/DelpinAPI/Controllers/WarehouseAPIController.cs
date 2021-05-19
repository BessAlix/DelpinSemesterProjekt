using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DelpinAPI.Data;
using System.Linq;

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

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllWarehouses()
        {
            var warehouses = await _context.Warehouse
                .AsNoTracking()
                .ToListAsync();
            return Ok(warehouses);
        }
        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetWarehouseCities()
        {
            var warehouses = await _context.Warehouse
                .AsNoTracking()
                .Select(w => w.City)
                .ToListAsync();
            return Ok(warehouses);
        }

    }
}
