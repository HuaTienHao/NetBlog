using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBlog.Web.Data;
using NetBlog.Web.Models.ViewModels;

namespace NetBlog.Web.Services
{
    public class UserService : IUserService
    {
        private readonly AuthDbContext _authDbContext;

        public UserService(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task<int> CountAsync()
        {
            return await _authDbContext.Users.CountAsync(x => x.Id != "2fa891b8-665e-4592-85bb-b34b0b3bbf3a");
        }

        public async Task<IEnumerable<IdentityUser>> GetAll(
            string? searchQuery,
            string? sortBy,
            string? sortDirection,
            int pageNumber = 1,
            int pageSize = 100)
        {
            var query = _authDbContext.Users.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x => x.UserName.Contains(searchQuery) ||
                                         x.Email.Contains(searchQuery));
            }
            var superAdminUser = await _authDbContext.Users.FirstOrDefaultAsync(x => x.Id == "2fa891b8-665e-4592-85bb-b34b0b3bbf3a");
            if (superAdminUser is not null)
            {
                query = query.Where(x => x.Id != superAdminUser.Id);
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

            // Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            query = query.Skip(skipResults).Take(pageSize);

            var users = await query.ToListAsync();
            
            return users;
        }
    }
}
