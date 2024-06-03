using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Web.Models.Domain;
using NetBlog.Web.Models.ViewModels;
using NetBlog.Web.Services.Interfaces;

namespace NetBlog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostService _blogPostService;
        private readonly IBlogPostLikeService _blogPostLikeService;
        private readonly IBlogPostCommentService _blogPostCommentService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public BlogsController(
            IBlogPostService blogPostService, 
            IBlogPostLikeService blogPostLikeService, 
            IBlogPostCommentService blogPostCommentService,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _blogPostService = blogPostService;
            _blogPostLikeService = blogPostLikeService;
            _blogPostCommentService = blogPostCommentService;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string urlHandle, 
            int pageSize = 5,
            int pageNumber = 1)
        {
            var liked = false;
            var blogPost = await _blogPostService.GetByUrlHandelAsync(urlHandle);
            var blogPostDetailViewModel = new BlogDetailViewModel();

            if (blogPost != null)
            {
                var totalLikes = await _blogPostLikeService.GetTotalLikes(blogPost.Id);

                if (_signInManager.IsSignedIn(User))
                {
                    var likesForBlog = await _blogPostLikeService.GetLikesForBlog(blogPost.Id);

                    var userId = _userManager.GetUserId(User);

                    if (userId != null)
                    {
                        var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));

                        liked = (likeFromUser != null);
                    }
                }

                // Get comments
                var totalRecords = await _blogPostCommentService.CountAsyncByBlogId(blogPost.Id);
                var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

                if (pageNumber > totalPages)
                    pageNumber--;

                if (pageNumber < 1)
                    pageNumber++;

                ViewBag.TotalPages = totalPages;
                ViewBag.PageSize = pageSize;
                ViewBag.PageNumber = pageNumber;
                ViewBag.UrlHandle = blogPost.UrlHandle;

                var blogCommentsDomainModel = await _blogPostCommentService.GetCommentByBlogIdAsync(blogPost.Id, pageNumber, pageSize);

                var blogCommentsForView = new List<BlogComment>();

                foreach (var blogComment in blogCommentsDomainModel)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Username = (await _userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName,
                        Description = blogComment.Description,
                        DateAdded = blogComment.DateAdded
                    });
                }

                // Mapping
                blogPostDetailViewModel = _mapper.Map<BlogDetailViewModel>(blogPost);
                blogPostDetailViewModel.TotalLikes = totalLikes;
                blogPostDetailViewModel.Liked = liked;
                blogPostDetailViewModel.Comments = blogCommentsForView;
            }

            return View(blogPostDetailViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailViewModel blogDetailViewModel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailViewModel.Id,
                    Description = blogDetailViewModel.CommentDescription,
                    UserId = Guid.Parse(_userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };

                await _blogPostCommentService.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailViewModel.UrlHandle, pageSize = 5, pageNumber = 1 });
            }

            return View();
        }
    }
}
