using BulkkyBook.Data;
using BulkkyBook.Data.Entities;

namespace BulkkyBook.Repositories.Identity;


public class FunctionRepository : IFunctionRepository
{
    private readonly ApplicationDbContext _context;
   
    public FunctionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Function> GetFunctions()
    {
        return _context.Functions
        .OrderBy(x => x.ParentId)
        .ThenBy(x => x.SortOrder)
        .ToList();
    }

    public Function? GetFunction(int id)
    {
        return _context.Functions.Find(id);
    }

    public Function AddFunction(Function function)
    {
        _context.Functions.Add(function);
        _context.SaveChanges();
        return function;
    }

    public Function UpdateFunction(Function function)
    {
        _context.Functions.Update(function);
        _context.SaveChanges();
        return function;
    }

    public void DeleteFunction(int id)
    {
        var function = GetFunction(id);
        if (function != null)
        {
            _context.Functions.Remove(function);
            _context.SaveChanges();
        }
    }


}