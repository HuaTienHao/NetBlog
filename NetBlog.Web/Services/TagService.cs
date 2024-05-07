using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetBlog.Web.Data;
using NetBlog.Web.Models.Domain;
using NetBlog.Web.Models.ViewModels;

namespace NetBlog.Web.Services
{
    public class TagService : ITagService
    {
        private readonly NetBlogDbContext _netBlogDbContext;
        private readonly IMapper _mapper;

        public TagService(NetBlogDbContext netBlogDbContext, IMapper mapper)
        {
            _netBlogDbContext = netBlogDbContext;
            _mapper = mapper;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await _netBlogDbContext.Tags.AddAsync(tag);
            await _netBlogDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tag = await _netBlogDbContext.Tags.FindAsync(id);

            if (tag != null)
            {
                _netBlogDbContext.Tags.Remove(tag);
                await _netBlogDbContext.SaveChangesAsync();
                return tag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _netBlogDbContext.Tags.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await _netBlogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existedTag = await _netBlogDbContext.Tags.FindAsync(tag.Id);
            if (existedTag != null)
            {
                existedTag.Name = tag.Name;
                existedTag.DisplayName = tag.DisplayName;

                await _netBlogDbContext.SaveChangesAsync();
                return existedTag;
            }
            return null;
        }
    }
}
