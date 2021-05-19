﻿using DelpinAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    }
}