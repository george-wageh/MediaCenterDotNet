using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediaCenterCore.Services;
using MediaCenterCore.ViewModels;
using System.Security.Claims;
using MediaCenterCore.Models;

namespace MediaCenterAdmin.Controllers
{
    [Authorize(Roles ="admin")]
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
            await LoadCategories();
            var mediasStatus = await MediaService.Query(q, categoryId);
            if (mediasStatus.IsSuccess)
            {
                return View(mediasStatus.Object);
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

        [HttpGet("[Action]")]
        public async Task<IActionResult> AddMediaVa(int id)
        {
            var token = MediaCenterCore.Security.StaticFilesTokenManager.GenerateToken(id.ToString(),"null",30);
            var model = new UploadMediaVaVM
            {
                Token = token,
                MediaId = id,
                MediaVa = null
            };
            return View(model);
        }



        public async Task LoadCategories() {
            var CategoriesOp = await CategoryService.GetAllAsync();
            if (CategoriesOp.IsSuccess)
            {
                ViewData["Categories"] = CategoriesOp.Object;
            }
        }


        [HttpGet("[Action]")]
        public async Task<IActionResult> Add(int? CategoryId)
        {
            if (CategoryId != null) {
                this.ViewData["CategoryId"] = CategoryId;
            }
            await LoadCategories();
            return this.View();
        }
        [HttpPost("[Action]")]
        public async Task<IActionResult> Add(MediaVM media)
        {

                var modelStateErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            if (ModelState.IsValid)
            {
                var response = await this.MediaService.AddAsync(media);
                if (response.IsSuccess)
                {
                    return this.RedirectToAction("Details", new { id = response.Object });
                }
            }
            else {
                await LoadCategories();
                return this.View(media);
            }
            return this.NotFound();
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

        [HttpGet("[Action]")]
        public async Task<IActionResult> Remove(int id) {
            var mediaStatus = await MediaService.GetMediaCard(id);
            if (mediaStatus.IsSuccess)
            {
                return PartialView("_confirmDelete", mediaStatus.Object);
            }
            return this.NotFound();
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> ConfirmRemove(int id)
        {
            var mediasStatus = await MediaService.DeleteAsync(id);
            if (mediasStatus.IsSuccess)
            {
                return this.RedirectToAction("index");
            }
            return this.NotFound();
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> Edit(int id)
        {
            await LoadCategories();

            var mediasStatus = await MediaService.GetMediaVmAsync(id);
            if (mediasStatus.IsSuccess)
            {
                return View("edit", mediasStatus.Object);
            }
            return this.NotFound();
        }

        [HttpPost("[Action]")]
        public async Task<IActionResult> Edit(MediaVM media , int photoFlag)
        {
            if (media != null)
            {
                var response = await this.MediaService.EditAsync(media.Id , media , photoFlag);
                if (response.IsSuccess)
                {
                    return this.RedirectToAction("Details", new { id = media.Id });
                }
            }
            return this.NotFound();
        }
    }
}
