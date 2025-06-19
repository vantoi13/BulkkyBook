using BulkkyBook.DTOS.Book;

namespace BulkkyBook.Services.Book
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
        Task<AuthorDto?> GetAuthorByIdAsync(string id);
        Task<AuthorDto> CreateAuthorAsync(CreateAuthorDto createDto);
        Task<AuthorDto> UpdateAuthorAsync(UpdateAuthorDto updateDto);
        Task DeleteAuthorAsync(string id);
    }
}