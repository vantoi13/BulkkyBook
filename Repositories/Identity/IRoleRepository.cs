using Microsoft.AspNetCore.Identity;

namespace BulkkyBook.Repositories.Identity;

public interface IRoleRepository
{
    ICollection<IdentityRole> GetRoles();
}