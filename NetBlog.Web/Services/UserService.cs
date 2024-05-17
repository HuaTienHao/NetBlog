using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBlog.Web.Data;

namespace NetBlog.Web.Services
{
    public class UserService : IUserService
    {
        private readonly AuthDbContext _authDbContext;

        public UserService(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }
        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var users = await _authDbContext.Users.ToListAsync();
            var superAdminUser = await _authDbContext.Users.FirstOrDefaultAsync(x => x.Email == "superadmin@netblog.com");

            if (superAdminUser is not null)
            {
                users.Remove(superAdminUser);
            }
            return users;
        }
    }
}
