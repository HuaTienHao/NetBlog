using Microsoft.EntityFrameworkCore;
using NetBlog.Web.Models.Domain;

namespace NetBlog.Web.Data
{
    public class NetBlogDbContext : DbContext
    {
        public NetBlogDbContext(DbContextOptions<NetBlogDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }
    }
}
