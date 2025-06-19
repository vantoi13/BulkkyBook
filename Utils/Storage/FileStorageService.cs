using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BulkkyBook.Utils.Storage
{
    public class FileStorageService : IStorageService
    {
        private readonly IWebHostEnvironment _env;
        public FileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var wwwRootPath = _env.WebRootPath;
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var directory = Path.Combine(wwwRootPath, folder);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var path = Path.Combine(directory, fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/{folder}/{fileName}".Replace("\\", "/");
        }

        public Task DeleteFileAsync(string filePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/').Replace("/", "\\"));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
            return Task.CompletedTask;
        }
    }
}