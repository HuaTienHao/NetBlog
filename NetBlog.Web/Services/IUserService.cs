using Microsoft.AspNetCore.Identity;

namespace NetBlog.Web.Services
{
    public interface IUserService
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
