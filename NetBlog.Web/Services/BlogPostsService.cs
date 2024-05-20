using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetBlog.Web.Data;
using NetBlog.Web.Models.Domain;

namespace NetBlog.Web.Services
{
    public class BlogPostsService : IBlogPostService
    {
        private readonly NetBlogDbContext _netBlogDbContext;
        private readonly IMapper _mapper;

        public BlogPostsService(NetBlogDbContext netBlogDbContext, IMapper mapper)
        {
            _netBlogDbContext = netBlogDbContext;
            _mapper = mapper;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _netBlogDbContext.BlogPosts.AddAsync(blogPost);
            await _netBlogDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await _netBlogDbContext.BlogPosts.FindAsync(id);
            if (existingBlog != null)
            {
                _netBlogDbContext.BlogPosts.Remove(existingBlog);
                await _netBlogDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync(string? searchQuery, string? searchByTag)
        {
            var query = _netBlogDbContext.BlogPosts.Include(x => x.Tags).AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x => x.Heading.Contains(searchQuery) || x.Title.Contains(searchQuery));
            }
            if (!string.IsNullOrWhiteSpace(searchByTag))
            {
                query = query.Where(x => x.Tags.Any(tag => tag.Name == searchByTag));
            }

            return await query.ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await _netBlogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetByUrlHandelAsync(string urlHandle)
        {
            return await _netBlogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await _netBlogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                _mapper.Map<BlogPost, BlogPost>(blogPost, existingBlog);
                await _netBlogDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }
    }
}
