using NetBlog.Web.Models.Domain;

namespace NetBlog.Web.Services.Interfaces
{
    public interface IBlogPostCommentService
    {
        Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment);
        Task<IEnumerable<BlogPostComment>> GetCommentByBlogIdAsync(
            Guid blogPostId,
            int pageNumber = 1,
            int pageSize = 100);
        Task<int> CountAsyncByBlogId(Guid blogPostId);
    }
}
