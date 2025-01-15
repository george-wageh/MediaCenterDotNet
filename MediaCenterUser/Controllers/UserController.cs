using MediaCenterUser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediaCenterCore.Services;
using System.Security.Claims;

namespace MediaCenterUser.Controllers
{
    [Authorize(Roles ="user")]
    public class UserController : Controller
    {
        public FavoritesService FavoritesService { get; }
        public WatchLaterService WatchLaterService { get; }
        public UserService UserService { get; }

        public UserController(FavoritesService favoritesService , WatchLaterService watchLaterService , UserService userService)
        {
            FavoritesService = favoritesService;
            WatchLaterService = watchLaterService;
            UserService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> AddWatchLater(int mediaId)
        {
            string userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;

            var saveOp = await WatchLaterService.AddWatchLater(mediaId, userId);
            if (saveOp.IsSuccess) { 
                return this.RedirectToAction("details", "media" ,new { id = mediaId });
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddFav(int mediaId)
        {
            string userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;

            var saveOp = await FavoritesService.AddFav(mediaId, userId);
            if (saveOp.IsSuccess)
            {
                return this.RedirectToAction("details", "media", new { id = mediaId });
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> RemoveWatchLater(int mediaId , string? redirectController , string? redirectAction)
        {
            string userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;

            var saveOp = await WatchLaterService.RemoveWatchLater(mediaId, userId);
            if (saveOp.IsSuccess)
            {
                return this.RedirectToAction("WatchLater", "user");
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFav(int mediaId, string? redirectController, string? redirectAction)
        {
            string userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;

            var saveOp = await FavoritesService.RemoveFav(mediaId, userId);
            if (saveOp.IsSuccess)
            {
                return this.RedirectToAction("Favorites", "user");
            }
            return NotFound();
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

        public async Task<IActionResult> WatchLater()
        {
            string userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
            var mediaWatchLaterOp = await WatchLaterService.GetAllAsync(userId);
            return View("WatchLater", mediaWatchLaterOp);
        }

        public async Task<IActionResult> Favorites()
        {
            string userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
            var mediaFavOp = await FavoritesService.GetAllAsync(userId);
            return View("Favorites", mediaFavOp);
        }
    }
}
