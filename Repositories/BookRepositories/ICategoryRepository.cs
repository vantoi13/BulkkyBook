using BulkkyBook.Data.Entities;

namespace BulkkyBook.Repositories.BookRepositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(string id);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
        Task<bool> ExistsByNameAsync(string name, string? excludeId = null);
        Task<IEnumerable<Category>> SearchAsync(string searchTerm);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<int> GetBookCountAsync(string categoryId);
    }
}