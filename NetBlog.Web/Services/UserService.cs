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
        public async Task<IEnumerable<IdentityUser>> GetAll(
            string? searchQuery,
            string? sortBy,
            string? sortDirection)
        {
            var query = _authDbContext.Users.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x => x.UserName.Contains(searchQuery) ||
                                         x.Email.Contains(searchQuery));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Username", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.UserName) : query.OrderBy(x => x.UserName);
                }

                if (string.Equals(sortBy, "Email", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Email) : query.OrderBy(x => x.Email);
                }
            }

            var users = await query.ToListAsync();
            var superAdminUser = await _authDbContext.Users.FirstOrDefaultAsync(x => x.Email == "superadmin@netblog.com");

            if (superAdminUser is not null)
            {
                users.Remove(superAdminUser);
            }
            return users;
        }
    }
}
