using Bloggie.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _authDbContext;
    public UserRepository(AuthDbContext authDbContext)
    {
        _authDbContext = authDbContext;
    }

    public async Task<IEnumerable<IdentityUser>> GetAll()
    {
        var users = await _authDbContext.Users.ToListAsync();
        var saUsers = await _authDbContext.Users.FirstOrDefaultAsync(u=> u.Email == "superadmin@bloggie.com");

        if (saUsers is not null)
        {
            users.Remove(saUsers);
        }

        return users;
    }
}
