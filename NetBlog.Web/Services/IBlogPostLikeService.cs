using NetBlog.Web.Models.Domain;

namespace NetBlog.Web.Services
{
    public interface IBlogPostLikeService
    {
        Task<int> GetTotalLikes(Guid blogPostId);
        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);
        Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
        Task<BlogPostLike?> UnlikeForBlog(Guid blogPostLikeId);
        Task<BlogPostLike?> GetLikeByBlogIdAndUserId(Guid blogPostId, Guid userId);
    }
}
