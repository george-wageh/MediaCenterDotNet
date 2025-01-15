using MediaCenterCore.Abstractions;

namespace MediaCenterUser.Services
{
    public class WebHostEnvironmentAccessor : IWebHostEnvironmentAccessor
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public WebHostEnvironmentAccessor(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string WebRootPath => _webHostEnvironment.WebRootPath;
        public string ContentRootPath => _webHostEnvironment.ContentRootPath;
    }
}
