using BulkkyBook.Repositories.Identity;
using BulkkyBook.Repositories.BookRepositories;

namespace BulkkyBook.Repositories;

public interface IUnitOfWork
{
    // Identity Repositories
    IUserRepository User { get; }
    IRoleRepository Role { get; }
    IPermissionRepository Permission { get; }
    ICommandRepository Command { get; }
    IFunctionRepository Function { get; }

    // Book Repositories
    IAuthorRepository Author { get; }
    ICategoryRepository Category { get; }
    IBookRepository Book { get; }
}
