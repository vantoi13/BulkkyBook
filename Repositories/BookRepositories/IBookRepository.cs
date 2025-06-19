using BulkkyBook.Data.Entities;

namespace BulkkyBook.Repositories.BookRepositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book?> GetByIdAsync(string id);
        Task<Book?> GetByIdWithDetailsAsync(string id);
        Task<Book> CreateAsync(Book book);
        Task<Book> UpdateAsync(Book book);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsByIsbnAsync(string isbn, string? excludeId = null);
        Task<IEnumerable<Book>> GetByAuthorAsync(string authorId);
        Task<IEnumerable<Book>> GetByCategoryAsync(string categoryId);
        Task<IEnumerable<Book>> SearchAsync(string searchTerm);
        Task<int> GetBookCountByAuthorAsync(string authorId);
    }
}