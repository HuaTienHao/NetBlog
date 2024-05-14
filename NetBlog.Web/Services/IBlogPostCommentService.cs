using NetBlog.Web.Models.Domain;

namespace NetBlog.Web.Services
{
    public interface IBlogPostCommentService
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
        Task<IEnumerable<BlogPostComment>> GetCommentByBlogIdAsync(Guid blogPostId);
    }
}
