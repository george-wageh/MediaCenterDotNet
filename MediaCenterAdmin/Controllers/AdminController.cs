using MediaCenterCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediaCenterCore.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MediaCenterAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("[Controller]")]
    public class AdminController : Controller
    {
        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            RoleManager = roleManager;
            UserManager = userManager;
        }

        public RoleManager<IdentityRole> RoleManager { get; }
        public UserManager<User> UserManager { get; }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> Admins()
        {
            var RoleId = RoleManager.Roles.Where(x => x.Name == "admin").Select(x => x.Id).FirstOrDefault();
            var UsersId = await UserManager.GetUsersInRoleAsync("admin");
            IEnumerable<AdminUser> adminUsers = UsersId.Select(x => new AdminUser { Id = x.Id, Email = x.Email, Phone = x.PhoneNumber });
            return View("Admins", adminUsers);
        }
        [HttpGet("[Action]")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> Add(string email, string phone)
        {
            var user = await UserManager.Users.FirstOrDefaultAsync(x => x.Email == email && x.PhoneNumber == phone);
            if (user != null)
            {
                await UserManager.AddToRolesAsync(user, new List<string> { "admin" });
            }
            return RedirectToAction("index");

        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> Remove(string id)
        {
            var user = await UserManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                return View("Remove", new AdminUser { Id = user.Id, Email = user.Email, Phone = user.PhoneNumber });
            }
            return RedirectToAction("index");
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> ConfirmRemove(string id)
        {
            var user = await UserManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user != null)
            {
                await UserManager.RemoveFromRolesAsync(user, new List<string> { "admin" });
            }
            return RedirectToAction("index");
        }
    }
}
