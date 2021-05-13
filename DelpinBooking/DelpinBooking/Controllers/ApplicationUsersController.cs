using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DelpinBooking.Data;
using DelpinBooking.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DelpinBooking.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public ApplicationUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManger)
        {
            _context = context;
            _userManager = userManger;
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var users = await _context.Users.Where(u=> u.Id == id).ToListAsync();
            return Ok(users);
        }
    }
}
