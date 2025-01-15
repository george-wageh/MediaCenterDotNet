using MediaCenterCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MediaCenterCore.Services;
using MediaCenterCore.ViewModels;

namespace MediaCenterAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("[Controller]")]
    public class CategoryController : Controller
    {
        public CategoryService CategoryService { get; }
        public MediaService MediaService { get; }

        public CategoryController(CategoryService categoryService , MediaService mediaService)
        {
            CategoryService = categoryService;
            MediaService = mediaService;
        }
        public async Task<IActionResult> Index()
        {
            var listOP = await CategoryService.GetAllAsync();
            if (listOP.IsSuccess)
            {
                return View("List", listOP.Object);
            }
            return NotFound();
        }


        [HttpGet("add")]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(CategoryVM category)
        {
            var categoryOP = await CategoryService.AddAsync(category);
            if (categoryOP.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet("[Action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var CategoryOP =  await CategoryService.GetAsync(id);
            if (CategoryOP.IsSuccess)
            {
                return View(CategoryOP.Object);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost("[Action]/{id}")]
        public async Task<IActionResult> Edit(CategoryVM category)
        {
            var CategoryOP = await CategoryService.UpdateAsync(category);
            if (CategoryOP.IsSuccess)
            {
                return View(CategoryOP.Object);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("[Action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var CategoryOP = await CategoryService.GetAsync(id);
            if (CategoryOP.IsSuccess)
            {
                return View(CategoryOP.Object);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var CategoryOP = await CategoryService.DeleteAsync(id);
            if (CategoryOP.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }


        [HttpGet("MediaList/{id}")]
        public async Task<IActionResult> Media(int id)
        {
            var MediaOp = await MediaService.Query(null, id);
            if(MediaOp.IsSuccess)
            {
                var CategoryOp = await this.CategoryService.GetAsync(id);
                if (CategoryOp.IsSuccess) {
                    ViewData["Category"] =  CategoryOp.Object.Name;
                }
            
                this.ViewData["CategoryId"] = id;
                return View("MediaList", MediaOp.Object);

            }
            return this.NotFound();
        }


    }
}
