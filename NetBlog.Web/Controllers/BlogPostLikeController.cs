using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Web.Models.Domain;
using NetBlog.Web.Models.ViewModels;
using NetBlog.Web.Services.Interfaces;

namespace NetBlog.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeService _blogPostLikeService;
        private readonly IMapper _mapper;

        public BlogPostLikeController(IBlogPostLikeService blogPostLikeService, IMapper mapper)
        {
            _blogPostLikeService = blogPostLikeService;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            var model = _mapper.Map<BlogPostLike>(addLikeRequest);

            await _blogPostLikeService.AddLikeForBlog(model);

            return Ok();
        }

        [HttpGet]
        [Route("{blogPostId:Guid}/totalLikes")]
        public async Task<IActionResult> GetTotalLikeForBlog([FromRoute] Guid blogPostId)
        {
            var totalLikes = await _blogPostLikeService.GetTotalLikes(blogPostId);

            return Ok(totalLikes);
        }

        [HttpPost]
        [Route("Unlike")]
        public async Task<IActionResult> Unlike([FromBody] UnlikeRequest unlikeRequest)
        {
            var blogPostLike = await _blogPostLikeService.GetLikeByBlogIdAndUserId(unlikeRequest.BlogPostId, unlikeRequest.UserId);
            if (blogPostLike != null)
            {
                await _blogPostLikeService.UnlikeForBlog(blogPostLike.Id);
                return Ok();
            }
            return BadRequest();
        }
    }
}
