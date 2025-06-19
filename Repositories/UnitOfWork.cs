using BulkkyBook.Repositories.Identity;
using BulkkyBook.Repositories.BookRepositories;

namespace BulkkyBook.Repositories;

public class UnitOfWork : IUnitOfWork
{
    // Identity Repositories
    public IUserRepository User { get; }
    public IRoleRepository Role { get; }
    public IPermissionRepository Permission { get; }
    public ICommandRepository Command { get; }
    public IFunctionRepository Function { get; }

    // Book Repositories
    public IAuthorRepository Author { get; }
    public ICategoryRepository Category { get; }
    public IBookRepository Book { get; }

    public UnitOfWork(
        IUserRepository user,
        IRoleRepository role,
        IPermissionRepository permission,
        ICommandRepository command,
        IFunctionRepository function,
        IAuthorRepository author,
        ICategoryRepository category,
        IBookRepository book)
    {
        User = user;
        Role = role;
        Permission = permission;
        Command = command;
        Function = function;
        Author = author;
        Category = category;
        Book = book;
    }
}