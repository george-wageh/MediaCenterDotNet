using Microsoft.AspNetCore.Mvc;

namespace MediaCenterAdmin.Controllers
{
    [Route("[Controller]")]
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult Index(string msg)
        {
            return View("Index",msg);
        }
    }
}
