using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DelpinAPI.Data;

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
            var warehouses = await _context.Warehouse.ToListAsync();
            return Ok(warehouses);
        }
    }
}
