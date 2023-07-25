using BeanSceneReservationApplication.Areas.Management.Models;
using BeanSceneReservationApplication.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace BeanSceneReservationApplication.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Roles = "Manager")]
    [Route("Management/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Get the users who belong to the desired roles
            var users = _userManager.GetUsersInRoleAsync("Staff").Result;
            var usersVM = new List<UserVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleText = "";
                if (roles.Contains("Manager"))
                {
                    roleText = "Manager";
                }
                else
                {
                    roleText = "Staff";
                }
                usersVM.Add(new UserVM
                {
                    Email = user.Email,
                    Role = roleText,
                });
            }

            return View(usersVM);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserVM user)
        {
            //Ensures email address for new user is unique
            var emailInUse = await _userManager.FindByEmailAsync(user.Email);

            if (emailInUse != null)
            {
                ModelState.AddModelError(nameof(user.Email), "Email is already in use");
            }

            if (user.Password != user.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(user.ConfirmPassword), "Password and password confirmation must be exactly the same");
            }

            if (user.Role != "Staff" && user.Role != "Manager")
            {
                ModelState.AddModelError(nameof(user.Role), "The new user must be Staff or Manager, an error has occured if any other option is available");
            }

            if (!Regex.Match(user.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$").Success)
            {
                ModelState.AddModelError(nameof(user.Password), "The password must contain at least 6 characters, including one lowercase letter, one uppercase letter, one digit, and one special character (@$!%*?&).");
            }

            if (ModelState.IsValid)
            {
                var validUser = new IdentityUser();
                validUser.UserName = user.Email;
                validUser.Email = user.Email;
                validUser.EmailConfirmed = true;

                await _userManager.CreateAsync(validUser, user.Password);
                await _userManager.AddToRoleAsync(validUser, "Staff");
                if (user.Role == "Manager")
                {
                    await _userManager.AddToRoleAsync(validUser, "Manager");
                }

                return RedirectToAction("Index", "User");
            }

            return View(user);
        }
    }
}
