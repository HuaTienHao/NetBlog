using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Web.Services.Interfaces;
using System.Net;

namespace NetBlog.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService _imagesService;

        public ImagesController(IImagesService imagesService)
        {
            _imagesService = imagesService;
        }
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            var imageURL = await _imagesService.UploadAsync(file);

            if (imageURL == null)
            {
                return Problem("Something went wrong", null, (int)HttpStatusCode.InternalServerError);
            }
            return new JsonResult(new { link = imageURL });
        }
    }
}
