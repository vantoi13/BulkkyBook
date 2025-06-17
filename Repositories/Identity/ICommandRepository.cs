using BulkkyBook.Data.Entities;

namespace  BulkkyBook.Repositories.Identity;

public interface ICommandRepository
{
    IList<Command> GetCommands();
    Command? GetCommand(int id);
    Command AddCommand(Command command);
    Command UpdateCommand(Command command);
    void DeleteCommand(int id);
}