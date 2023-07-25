using BeanSceneReservationApplication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeanSceneReservationApplication.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TableController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet,Route("tables")]

        public async Task<ActionResult<IEnumerable<string>>> GetNames()
        {
            var tables = await _context.RestaurantTables.Select(t => t.Name).ToListAsync();
            Console.WriteLine(tables);
            return tables;
        }
    }
}
