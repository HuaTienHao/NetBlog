using NetBlog.Web.Models.Domain;

namespace NetBlog.Web.Services
{
    public interface IBlogPostService
    {
        Task<IEnumerable<BlogPost>> GetAllAsync(
            string? searchQuery = null, 
            string? searchByTag = null,
            int pageNumber = 1,
            int pageSize = 100);
        Task<BlogPost?> GetAsync(Guid id);
        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost?> UpdateAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteAsync(Guid id);
        Task<BlogPost?> GetByUrlHandelAsync(string urlHandle); 
        Task<int> CountAsync();
    }
}
