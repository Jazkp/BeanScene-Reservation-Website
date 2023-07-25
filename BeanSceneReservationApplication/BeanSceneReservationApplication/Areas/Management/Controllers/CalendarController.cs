using BeanSceneReservationApplication.Data;
using BeanSceneReservationApplication.Models.ReservationModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeanSceneReservationApplication.Areas.Management.Controllers

{
    [Area("Management")]
    [Authorize(Roles = "Manager")]
    [Route("Management/[controller]/[action]")]
    public class CalendarController : Controller
    {
        private readonly ILogger<CalendarController> _logger;
        private readonly ApplicationDbContext _context;

        public CalendarController(ApplicationDbContext context, ILogger<CalendarController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
