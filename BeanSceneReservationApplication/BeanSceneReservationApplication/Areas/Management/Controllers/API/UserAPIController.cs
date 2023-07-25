using BeanSceneReservationApplication.Areas.Management.Models;
using BeanSceneReservationApplication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeanSceneReservationApplication.Areas.Management.Controllers.API
{
    [Route("/Management/API/User")]
    [ApiController]
    public class UserAPIController : Controller
    {

        private readonly ILogger<UserController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserAPIController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserVM user)
        {
            var databaseUser = await _userManager.FindByEmailAsync(user.Email);
            if (databaseUser == null)
            {
                return BadRequest();
            }
            var databaseRoles = await _userManager.GetRolesAsync(databaseUser);
            await _userManager.RemoveFromRolesAsync(databaseUser, databaseRoles);
            await _userManager.AddToRoleAsync(databaseUser, "Staff");
            if (user.Role == "Manager")
            {
                await _userManager.AddToRoleAsync(databaseUser, "Manager");
            }
            return Ok();
        }
    }
}
