using BeanSceneReservationApplication.Data;
using BeanSceneReservationApplication.Models;
using BeanSceneReservationApplication.Models.ReservationListModel;
using BeanSceneReservationApplication.Models.ReservationModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeanSceneReservationApplication.Areas.Staff.Controllers
{
    [Area("Staff")]
    [Authorize(Roles = "Staff")]
    [Route("Staff/list/[action]")]
    public class ReservationListController : Controller
    {
        private readonly ILogger<ReservationListController> _logger;
        private readonly ApplicationDbContext _context;

        public ReservationListController(ApplicationDbContext context, ILogger<ReservationListController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Indexes()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
