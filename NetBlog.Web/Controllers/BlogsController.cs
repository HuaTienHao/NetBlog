using Microsoft.AspNetCore.Mvc;
using NetBlog.Web.Services;

namespace NetBlog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostService _blogPostService;

        public BlogsController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blogPost = await _blogPostService.GetByUrlHandelAsync(urlHandle);

            return View(blogPost);
        }
    }
}
