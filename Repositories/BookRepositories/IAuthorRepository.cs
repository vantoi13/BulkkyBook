using BulkkyBook.Data.Entities;
using BulkkyBook.DTOS.Book;
using BulkkyBook.Utils;

namespace BulkkyBook.Repositories.BookRepositories
{
    public interface IAuthorRepository
    {
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByEmailAsync(string email);
        Task<Author> CreateAuthorAsync(Author author);
        Task<Author?> GetAuthorByIdAsync(string id);
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<PaginatedList<Author>> GetPaginatedAuthorsAsync(string? searchTerm, int pageIndex = 1, int pageSize = 10);
        Task<Author> UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(string id);
    }
}