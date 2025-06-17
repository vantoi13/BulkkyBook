using BulkkyBook.Repositories.Identity;

namespace BulkkyBook.Repositories;

public class UnitOfWork : IUnitOfWork
{

    public IUserRepository User { get; }
    public IRoleRepository Role { get; }
    public IPermissionRepository Permission { get; }
    public ICommandRepository Command { get; }
    public IFunctionRepository Function { get; }

    public UnitOfWork(IUserRepository user,
    IRoleRepository role,
    IPermissionRepository permission,
    ICommandRepository command,
    IFunctionRepository function)
    {
        User = user;
        Role = role;
        Permission = permission;
        Command = command;
        Function = function;
    }


}
