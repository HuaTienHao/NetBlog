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

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _netBlogDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
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
