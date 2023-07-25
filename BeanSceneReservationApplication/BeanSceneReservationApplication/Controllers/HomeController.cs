using BeanSceneReservationApplication.Data;
using BeanSceneReservationApplication.Models;
using BeanSceneReservationApplication.Models.ReservationModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MongoDB.Driver.Linq;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Text.Json;

namespace BeanSceneReservationApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult RedirectUser()
        {
            if (User.IsInRole("Manager"))
            {
                return RedirectToAction("Index", "Home", new { area = "Management" });
            }
            else if (User.IsInRole("Staff"))
            {
                return RedirectToAction("Index", "Home", new { area = "Staff" });
            }
            else if (User.IsInRole("Member"))
            {
                return RedirectToAction("Index", "Home", new { area = "Membership" });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Team()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}