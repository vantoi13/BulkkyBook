using Microsoft.AspNetCore.Http;

namespace BulkkyBook.Utils.Storage
{
    public interface IStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
        Task DeleteFileAsync(string filePath);
    }
}