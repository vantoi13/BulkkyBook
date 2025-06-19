using BulkkyBook.DTOS.Book;

namespace BulkkyBook.Services.Book
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(string id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto);
        Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto updateDto);
        Task DeleteCategoryAsync(string id);
        Task<bool> CategoryExistsAsync(string id);
        Task<bool> CategoryHasChildrenAsync(string id);
        Task<bool> CategoryHasBooksAsync(string id);
        Task<IEnumerable<CategoryDto>> GetParentCategoriesAsync();
        Task<IEnumerable<CategoryDto>> SearchCategoriesAsync(string searchTerm);
    }
}