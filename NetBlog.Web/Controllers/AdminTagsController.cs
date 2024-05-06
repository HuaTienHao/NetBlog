using Microsoft.AspNetCore.Mvc;
using NetBlog.Web.Data;
using NetBlog.Web.Models.Domain;
using NetBlog.Web.Models.ViewModels;

namespace NetBlog.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly NetBlogDbContext _netBlogDbContext;

        public AdminTagsController(NetBlogDbContext netBlogDbContext)
        {
            _netBlogDbContext = netBlogDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            Tag tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };

            _netBlogDbContext.Tags.Add(tag);
            _netBlogDbContext.SaveChanges();

            return View("Add");
        }
    }
}
