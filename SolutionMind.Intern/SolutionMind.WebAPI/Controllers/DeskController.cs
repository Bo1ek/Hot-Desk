using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareMind.Infrastructure.Entities;
using SoftwareMind.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SolutionMind.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]  
    public class DeskController : Controller
    {
        private readonly ApplicationDbContext _context;

        [HttpPost]
        public async Task<ActionResult<Location>> CreateLocation(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return Ok(await _context.Locations.ToListAsync());
        }
    }
}
