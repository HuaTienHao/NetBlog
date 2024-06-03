
using Microsoft.EntityFrameworkCore;
using NetBlog.Web.Data;
using NetBlog.Web.Models.Domain;
using NetBlog.Web.Services.Interfaces;

namespace NetBlog.Web.Services
{
    public class BlogPostLikeService : IBlogPostLikeService
    {
        private readonly NetBlogDbContext _netBlogDbContext;

        public BlogPostLikeService(NetBlogDbContext netBlogDbContext)
        {
            _netBlogDbContext = netBlogDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await _netBlogDbContext.BlogPostLike.AddAsync(blogPostLike);
            await _netBlogDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<BlogPostLike?> GetLikeByBlogIdAndUserId(Guid blogPostId, Guid userId)
        {
            return await _netBlogDbContext.BlogPostLike.FirstOrDefaultAsync(x => x.BlogPostId == blogPostId && x.UserId == userId);
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await _netBlogDbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await _netBlogDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
        }

        public async Task<BlogPostLike?> UnlikeForBlog(Guid blogPostLikeId)
        {
            var like = await _netBlogDbContext.BlogPostLike.FindAsync(blogPostLikeId);

            if (like != null)
            {
                _netBlogDbContext.BlogPostLike.Remove(like);
                await _netBlogDbContext.SaveChangesAsync();
                return like;
            }

            return null;
        }
    }
}
