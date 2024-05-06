using Microsoft.EntityFrameworkCore;
using NetBlog.Web.Models.Domain;

namespace NetBlog.Web.Data
{
    public class NetBlogDbContext : DbContext
    {
        public NetBlogDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
