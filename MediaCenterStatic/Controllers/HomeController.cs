using MediaCenterCore.Data;
using MediaCenterCore.Models;
using MediaCenterCore.Shared;
using MediaCenterStatic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MediaCenterStatic.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IWebHostEnvironment WebHostEnvironment { get; }
        public MediaCenterDbContext MediaCenterDbContext { get; }

        public HomeController(ILogger<HomeController> logger , IWebHostEnvironment webHostEnvironment , MediaCenterDbContext mediaCenterDbContext)
        {
            _logger = logger;
            WebHostEnvironment = webHostEnvironment;
            MediaCenterDbContext = mediaCenterDbContext;
        }
        [HttpPost("[Action]")]
        public async Task<ActionResult<OperationResult<string>>> UploadImage()
        {
            var image = Request.Form.Files[0];

            var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(WebHostEnvironment.WebRootPath, "media-images", fileName);


            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    await image.CopyToAsync(fileStream);

                    return Ok(new OperationResult<string>
                    {
                        IsSuccess = true,
                        Object = fileName

                    });
                }
                catch (Exception e)
                {
                    return BadRequest();
                }

            }
        }

        [HttpPost("[Action]")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<OperationResult<string>>> UploadMediaVa(string token)
        {
            var (isValid , mediaId , fileName) = MediaCenterCore.Security.StaticFilesTokenManager.ValidateToken(token);
            if (!isValid)
            {
                return Redirect(MediaCenterCore.Shared.DomainsNames.HostAdminDomain + $"/Error?msg=Error in token");

            }

            var image = Request.Form.Files.FirstOrDefault();
            if (image == null || image.Length == 0)
            {
                return Redirect(MediaCenterCore.Shared.DomainsNames.HostAdminDomain + $"/Error?msg=No file was uploaded.");
            }

 

            fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(WebHostEnvironment.WebRootPath, "media-va", fileName);

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                var MediaVa = await MediaCenterDbContext.Set<MediaVa>().Where(x => x.MediaId == int.Parse(mediaId)).FirstOrDefaultAsync();
                if (MediaVa == null)
                {
                    await MediaCenterDbContext.Set<MediaVa>().AddAsync(new MediaVa
                    {
                        MediaId = int.Parse(mediaId),
                        FileName = fileName
                    });
                }
                else {
                    MediaVa.FileName = fileName;
                }
                await MediaCenterDbContext.SaveChangesAsync();
                return Redirect(MediaCenterCore.Shared.DomainsNames.HostAdminDomain + $"/Media/Details?id={mediaId}");

            }
            catch (Exception e)
            {
                return Redirect(MediaCenterCore.Shared.DomainsNames.HostAdminDomain + $"/Error?msg={e.Message}");
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}