using MediaCenterAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediaCenterCore.Services;
using System.Security.Claims;

namespace MediaCenterAdmin.Controllers
{
    [Authorize(Roles ="admin")]
    public class UserController : Controller
    {
        public UserService UserService { get; }

        public UserController(UserService userService)
        {
            UserService = userService;
        }
        public async Task<IActionResult> LoadProfileModal()
        {
            string userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
            var ProfileVmOp = await UserService.GetUserInfo(userId);
            if (ProfileVmOp.IsSuccess) {
                return PartialView("_ProfilePartial", ProfileVmOp.Object);
            }
            return PartialView("_ProfilePartial", null);
        }

    }
}
