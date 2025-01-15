using MediaCenterCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using MediaCenterCore.ViewModels;

namespace MediaCenterAdmin.Controllers
{
    [Authorize(Roles ="owner")]
    [Route("[Controller]")]
    public class RoleController : Controller
    {
        public RoleManager<IdentityRole> RoleManager { get; }
        public UserManager<User> UserManager { get; }

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<User> userManager)
        {
            RoleManager = roleManager;
            UserManager = userManager;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            var roles = RoleManager.Roles.Select(x => new RoleVM
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            if (roles != null)
            {
                return View(roles);
            }
            return NotFound();
        }
        [HttpGet("[Action]")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpGet("[Action]")]
        public IActionResult Remove(string Id)
        {
            var role = RoleManager.Roles.Select(x => new RoleVM
            {
                Id = x.Id,
                Name = x.Name
            }).First(x => x.Id == Id);
            if(role != null)
            {
                return View(role);

            }
            return RedirectToAction("index");
        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> Add(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                 var re = await RoleManager.CreateAsync(new IdentityRole
                {
                    Name = roleVM.Name,
                });
                if (re.Succeeded) {
                    return RedirectToAction("index");

                }
            }
            return View(roleVM);
        }
        [HttpPost("[Action]")]
        public IActionResult Remove(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                RoleManager.DeleteAsync(new IdentityRole
                {
                    Id = roleVM.Id
                });
                return RedirectToAction("index");
            }
            else
            {
                return View(roleVM);
            }
        }
       

    }

}
