using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data;

public class AuthDbContext : IdentityDbContext
{
    public AuthDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var adminRoleId = "b616b6c7-5fc8-4cc7-aa45-3e3eee629578";
        var saRoleId = "bb0a450f-21ca-4dd2-bbdf-dd769572ea29";
        var userRoleId = "0b3b1237-db4b-4380-b348-014feaf7ac31";

        //Seed Roles(User, Admin, SA)
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
                Id = saRoleId,
                ConcurrencyStamp = saRoleId
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "User",
                Id = userRoleId,
                ConcurrencyStamp = userRoleId
            }
        };

        builder.Entity<IdentityRole>().HasData(roles);

        //Seed SA
        var saAdminId = "6e4ffda1-7a7e-4f25-a30e-3182a0b67dc2";

        var saAdminUser = new IdentityUser
        {
            UserName = "superadmin@bloggie.com",
            Email = "superadmin@bloggie.com",
            NormalizedEmail = "superadmin@bloggie.com".ToUpper(),
            NormalizedUserName = "superadmin@bloggie.com".ToUpper(),
            Id = saAdminId,
        };

        saAdminUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(saAdminUser, "Superadmin@123");

        builder.Entity<IdentityUser>().HasData(saAdminUser);

        //Add All roles to SA
        var saRoles = new List<IdentityUserRole<string>>
        {
            new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = saAdminId,
            },
            new IdentityUserRole<string>
            {
                RoleId = saRoleId,
                UserId = saAdminId,
            },
            new IdentityUserRole<string>
            {
                RoleId = userRoleId,
                UserId = saAdminId,
            }
        };

        builder.Entity<IdentityUserRole<string>>().HasData(saRoles);
    }
}
