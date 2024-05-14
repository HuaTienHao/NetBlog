using Microsoft.EntityFrameworkCore;
using NetBlog.Web.Data;
using NetBlog.Web.Models.Domain;

namespace NetBlog.Web.Services
{
    public class BlogPostCommentService : IBlogPostCommentService
    {
        private readonly NetBlogDbContext _netBlogDbContext;

        public BlogPostCommentService(NetBlogDbContext netBlogDbContext)
        {
            _netBlogDbContext = netBlogDbContext;
        }
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await _netBlogDbContext.BlogPostComment.AddAsync(blogPostComment);
            await _netBlogDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentByBlogIdAsync(Guid blogPostId)
        {
            return await _netBlogDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
