using BulkkyBook.Data;
using BulkkyBook.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulkkyBook.Repositories.BookRepositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(string id)
        {
            return await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book?> GetByIdWithDetailsAsync(string id)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> CreateAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task DeleteAsync(string id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.Books.AnyAsync(b => b.Id == id);
        }

        public async Task<bool> ExistsByIsbnAsync(string isbn, string? excludeId = null)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(excludeId))
            {
                query = query.Where(b => b.Id != excludeId);
            }

            var lowerIsbn = isbn.ToLower();
            return await query.AnyAsync(b => b.ISBN.ToLower() == lowerIsbn);
        }

        public async Task<IEnumerable<Book>> GetByAuthorAsync(string authorId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.AuthorId == authorId)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetByCategoryAsync(string categoryId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.CategoryId == categoryId)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.Title.Contains(searchTerm) ||
                           b.Description.Contains(searchTerm) ||
                           b.ISBN.Contains(searchTerm) ||
                           b.Author.Name.Contains(searchTerm) ||
                           b.Category.Name.Contains(searchTerm))
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<int> GetBookCountByAuthorAsync(string authorId)
        {
            return await _context.Books.CountAsync(b => b.AuthorId == authorId);
        }
    }
}