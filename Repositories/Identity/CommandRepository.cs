using BulkkyBook.Data;
using BulkkyBook.Data.Entities;

namespace BulkkyBook.Repositories.Identity;


public class CommandRepository : ICommandRepository
{
    private readonly ApplicationDbContext _context;

    public CommandRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Command> GetCommands()  
    {
        return _context.Commands
            .OrderBy(x => x.Name)
            .ToList();
    }

    public Command? GetCommand(int id)
    {
        return _context.Commands.Find(id);
    }

    public Command AddCommand(Command command)
    {
        _context.Commands.Add(command);
        _context.SaveChanges();
        return command;
    }

    public Command UpdateCommand(Command command)
    {
        _context.Commands.Update(command);
        _context.SaveChanges();
        return command;
    }

    public void DeleteCommand(int id)
    {
        var command = GetCommand(id);
        if (command != null)
        {
            _context.Commands.Remove(command);
            _context.SaveChanges();
        }
    }


}