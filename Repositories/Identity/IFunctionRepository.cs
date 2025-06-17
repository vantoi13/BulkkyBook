using BulkkyBook.Data.Entities;

namespace BulkkyBook.Repositories.Identity
{
    public interface IFunctionRepository
    {
        IList<Function> GetFunctions();
        Function? GetFunction(int id);
        Function AddFunction(Function function);
        Function UpdateFunction(Function function); 
        void DeleteFunction(int id);
    }
}