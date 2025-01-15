using MediaCenterCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MediaCenterCore.ViewModels;

namespace MediaCenterUser.Controllers
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
        [HttpGet("[Action]")]
        public async Task<IActionResult> Register()
        {
            await this.SignInManager.SignOutAsync();
            return View();
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
                }
                ModelState.AddModelError("Server", "Email or password is incorrect");

            }
            else
            {
                return View(loginVM);
            }
            return View(loginVM);
        }


        [HttpPost("[Action]")]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = registerVM.EmailAddress,
                    PhoneNumber = registerVM.PhoneNumber,
                    FullName = registerVM.FullName,
                    UserName = Guid.NewGuid().ToString()
                };
                var x = await UserManager.Users.FirstOrDefaultAsync(x => x.Email == registerVM.EmailAddress);
                if (x == null)
                {
                    var result = await UserManager.CreateAsync(user, registerVM.Password);
                    var result2 = await UserManager.AddToRoleAsync(user, "user");
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    this.ModelState.AddModelError("Email", "Email is used");
                    return View(registerVM);
                }

            }
            return View(registerVM);

        }
    }
}
