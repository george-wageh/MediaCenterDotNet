using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediaCenterCore.Services;
using MediaCenterCore.ViewModels;
using System.Security.Claims;

namespace MediaCenterUser.Controllers
{
    [Authorize(Roles ="user")]
    [Route("[Controller]")]
    public class MediaController : Controller
    {
        public MediaService MediaService { get; }
        public CategoryService CategoryService { get; }

        public MediaController(MediaService mediaService , CategoryService categoryService)
        {
            MediaService = mediaService;
            CategoryService = categoryService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? q , int? categoryId)
        {
            var mediasStatus = await MediaService.Query(q, categoryId);
            if (mediasStatus.IsSuccess)
            {
                return View(mediasStatus.Object);
            }
            return this.NotFound();
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> LoadCategories()
        {
            var CategoriesOp = await CategoryService.GetAllAsync();
            if (CategoriesOp.IsSuccess)
            {
                return this.PartialView("_LoadCategories", CategoriesOp.Object);
            }
            return this.NotFound();
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> Details(int id)
        {

            string userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;

            var mediaRes = await MediaService.GetDetailsAsync(id, userId);
            if (mediaRes.IsSuccess)
            {
                return View(mediaRes.Object);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> ViewMedia(int id)
        {
            string userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;

            var mediaRes = await MediaService.GetViewMediaVMAsync(id);
            if (mediaRes.IsSuccess)
            {
                return View(mediaRes.Object);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddLike(int mediaId, bool isLike) {

            string userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value;
            var addLikeRes = await MediaService.ToggleLikeAsync(mediaId, userId);
            return this.RedirectToAction("Details", new { id = mediaId });

        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> AddComment(CommentVM CommentVM)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var response = await MediaService.CommentsService.AddCommentAsync(CommentVM, userId);
            if (response.IsSuccess)
            {
                return this.RedirectToAction("Details", new { id = CommentVM.MediaId });
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> RemoveComment(int mediaId, int CommentId)
        {
            await MediaService.CommentsService.RemoveCommentAsync(CommentId);
            return this.RedirectToAction("Details", new { id = mediaId });
        }

    }
}
