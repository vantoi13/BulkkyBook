using BulkkyBook.Data.Entities;

namespace BulkkyBook.Repositories.Identity
{
    public interface IPermissionRepository
    {
        IList<Permission> GetPermissionsByRole(string roleId);
        void SavePermissions(string roleId, List<Permission> permissions);
        void RemovePermissions(string roleId);
    }
}
