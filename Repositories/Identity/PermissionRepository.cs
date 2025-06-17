using BulkkyBook.Data;
using BulkkyBook.Data.Entities;

namespace BulkkyBook.Repositories.Identity;

public class PermissionRepository : IPermissionRepository
{
    private readonly ApplicationDbContext _context;

    public PermissionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Permission> GetPermissionsByRole(string roleId)
    {
        return _context.Permissions
        .Where(p => p.RoleId == roleId)
        .ToList();
    }

    public void SavePermissions(string roleId, List<Permission> permissions)
    {
         _context.Permissions.AddRange(permissions);
        _context.SaveChanges();
    }

    public void RemovePermissions(string roleId)
    { 
        var permissions = _context.Permissions
        .Where(p => p.RoleId == roleId).ToList();
        _context.Permissions.RemoveRange(permissions);
        _context.SaveChanges();
    }
}