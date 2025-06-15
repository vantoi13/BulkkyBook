using BulkkyBook.Data;
using Microsoft.AspNetCore.Identity;

namespace BulkkyBook.Repositories.Identity;

public class RoleRepository : IRoleRepository
{   
    private readonly ApplicationDbContext _context;
    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public ICollection<IdentityRole> GetRoles()
    {
        return _context.Roles.ToList();
    }
}