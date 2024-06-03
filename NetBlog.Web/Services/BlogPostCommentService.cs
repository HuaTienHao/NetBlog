using Microsoft.EntityFrameworkCore;
using NetBlog.Web.Data;
using NetBlog.Web.Models.Domain;
using NetBlog.Web.Services.Interfaces;

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

        public Task<int> CountAsyncByBlogId(Guid blogPostId)
        {
            return _netBlogDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId).CountAsync();
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentByBlogIdAsync(
            Guid blogPostId, 
            int pageNumber = 1,
            int pageSize = 100)
        {
            var query = _netBlogDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId).OrderByDescending(x => x.DateAdded).AsQueryable();

            //Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            return await query.ToListAsync();
        }
    }
}
