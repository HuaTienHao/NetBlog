using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NetBlog.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Roles (User, Admin, SuperAdmin)
            var adminRoleId = "07230d08-368c-4141-bd88-e3aa68b898ee";
            var superAdminRoleId = "227c85b1-3abf-48ce-be85-5eff2a946ff7";
            var userRoleId = "97d6a36c-9fe2-4f92-bb7f-3a5cf7f2c108";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp= userRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            // Seed SuperAdmin
            var superAdminId = "2fa891b8-665e-4592-85bb-b34b0b3bbf3a";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@netblog.com",
                Email = "superadmin@netblog.com",
                NormalizedEmail = "superadmin@netblog.com".ToUpper(),
                NormalizedUserName = "superadmin@netblog.com".ToUpper(),
                Id = superAdminId,
            };
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "SuperAdmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // Add all roles to SuperAdmin
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
