using Microsoft.EntityFrameworkCore;
using NetBlog.Web.Data;
using NetBlog.Web.Models.Domain;

namespace NetBlog.Web.Services
{
    public class BlogPostsService : IBlogPostService
    {
        private readonly NetBlogDbContext _netBlogDbContext;

        public BlogPostsService(NetBlogDbContext netBlogDbContext)
        {
            _netBlogDbContext = netBlogDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _netBlogDbContext.BlogPosts.AddAsync(blogPost);
            await _netBlogDbContext.SaveChangesAsync();
            return blogPost;
        }

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _netBlogDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public Task<BlogPost?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
