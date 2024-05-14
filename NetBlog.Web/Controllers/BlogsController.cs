using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Web.Models.ViewModels;
using NetBlog.Web.Services;

namespace NetBlog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostService _blogPostService;
        private readonly IBlogPostLikeService _blogPostLikeService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public BlogsController(
            IBlogPostService blogPostService, 
            IBlogPostLikeService blogPostLikeService, 
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _blogPostService = blogPostService;
            _blogPostLikeService = blogPostLikeService;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
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

                blogPostDetailViewModel = _mapper.Map<BlogDetailViewModel>(blogPost);
                blogPostDetailViewModel.TotalLikes = totalLikes;
                blogPostDetailViewModel.Liked = liked;
            }

            return View(blogPostDetailViewModel);
        }
    }
}
