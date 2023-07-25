using BeanSceneReservationApplication.Data;
using BeanSceneReservationApplication.Models;
using BeanSceneReservationApplication.Models.ReservationModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MongoDB.Driver.Linq;
using System.Data;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;

namespace BeanSceneReservationApplication.Areas.ManagementControllers
{
    [Area("Management")]
    [Authorize(Roles = "Manager")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
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