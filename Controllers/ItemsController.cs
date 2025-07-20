using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using store_api.Data;
using store_api.Models;

namespace store_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbItem _context;

        public ItemsController(ApplicationDbItem context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> Index()
        {
            var items = await _context.Items.ToListAsync();
            return Ok(items);
        }
    }
}