using Microsoft.AspNetCore.Mvc;
using NetBlog.Web.Models;
using NetBlog.Web.Models.ViewModels;
using NetBlog.Web.Services;
using System.Diagnostics;

namespace NetBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostService _blogPostService;
        private readonly ITagService _tagService;

        public HomeController(ILogger<HomeController> logger, IBlogPostService blogPostService, ITagService tagService)
        {
            _logger = logger;
            _blogPostService = blogPostService;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index(
            string? searchQuery, 
            string? searchByTag,
            int pageSize = 6,
            int pageNumber = 1)
        {
            var totalRecords = await _blogPostService.CountAsync(true);
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if (pageNumber > totalPages)
                pageNumber--;

            if (pageNumber < 1)
                pageNumber++;

            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SearchByTag = searchByTag;

            var blogPosts = await _blogPostService.GetAllAsync(searchQuery, searchByTag, pageNumber, pageSize, true);
            var tags = await _tagService.GetAllAsync();
            var model = new HomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
