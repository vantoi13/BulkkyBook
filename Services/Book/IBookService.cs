using BulkkyBook.DTOS.Book;
using BulkkyBook.Utils;

namespace BulkkyBook.Services.Book
{
    public interface IBookService
    {
        Task<BulkkyBook.Utils.PaginatedList<BookDto>> GetAllBooksAsync(string? searchTerm = null, int pageIndex = 1, int pageSize = 5);
        Task<BookDto?> GetBookByIdAsync(string id);
        Task<BookDto> CreateBookAsync(CreateBookDto createDto);
        Task<BookDto> UpdateBookAsync(UpdateBookDto updateDto);
        Task DeleteBookAsync(string id);
        Task<bool> BookExistsAsync(string id);
        Task<BulkkyBook.Utils.PaginatedList<BookDto>> GetBooksByAuthorAsync(string authorId, int pageIndex = 1, int pageSize = 10);
        Task<BulkkyBook.Utils.PaginatedList<BookDto>> GetBooksByCategoryAsync(string categoryId, int pageIndex = 1, int pageSize = 10);
        Task<IEnumerable<BookDto>> SearchBooksAsync(string searchTerm);
    }
}