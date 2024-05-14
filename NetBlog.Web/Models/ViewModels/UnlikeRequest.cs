namespace NetBlog.Web.Models.ViewModels
{
    public class UnlikeRequest
    {
        public Guid BlogPostId { get; set; }
        public Guid UserId { get; set; }
    }
}
