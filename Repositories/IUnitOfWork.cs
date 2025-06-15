using BulkkyBook.Repositories.Identity;

namespace BulkkyBook.Repositories;

public interface IUnitOfWork
{
    IUserRepository User { get; }
    IRoleRepository Role { get; }
}
