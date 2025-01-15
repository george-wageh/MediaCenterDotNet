using MediaCenterCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MediaCenterCore.ViewModels;

namespace MediaCenterAdmin.Controllers
{
    [Route("[Controller]")]
    public class AccountController : Controller
    {
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("[Action]")]
        public async Task<IActionResult> Login()
        {
            await this.SignInManager.SignOutAsync();
            return View();
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> Signout()
        {
            await this.SignInManager.SignOutAsync();
            return RedirectToAction("Login", "account");
        }
    
        [HttpPost("[Action]")]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {

                var user = await UserManager.Users.FirstOrDefaultAsync(x => x.Email == loginVM.EmailAddress);
                if (user != null)
                {
                    var re = await SignInManager.CheckPasswordSignInAsync(user, password: loginVM.Password, false);
                    if (re.Succeeded)
                    {
                        var isAdmin = await UserManager.IsInRoleAsync(user, "admin");
                        if (isAdmin)
                        {
                            var userClaims = new List<Claim>
                            {
                            new Claim("Email", user.Email),
                            new Claim("FullName", user.FullName),
                            new Claim("PhoneNumber", user.PhoneNumber)
                            };

                            var claimsIdentity = new ClaimsIdentity(userClaims, "CustomIdentity");

                            await SignInManager.SignInWithClaimsAsync(user, loginVM.RememberMe, claimsIdentity.Claims);

                            return this.RedirectToAction("index", "home");
                        }
                        else
                        {
                            ModelState.AddModelError("Server", "Access denied");

                        }
                    }
                }
                else {
                    ModelState.AddModelError("Server", "Email or password is incorrect");
                }
            }
            else
            {
                return View(loginVM);
            }
            return View(loginVM);
        }

    }
}
