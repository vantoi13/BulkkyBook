using BulkkyBook.Data;
using BulkkyBook.Data.Entities;
using BulkkyBook.Utils;
using Microsoft.EntityFrameworkCore;

namespace BulkkyBook.Repositories.BookRepositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Authors.AnyAsync(a => a.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Authors.AnyAsync(a => a.Email.ToLower() == email.ToLower());
        }

        public async Task<Author> CreateAuthorAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author?> GetAuthorByIdAsync(string id)
        {
            return await _context.Authors.FindAsync(id);
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors.OrderBy(a => a.Name).ToListAsync();
        }

        public async Task<PaginatedList<Author>> GetPaginatedAuthorsAsync(string? searchTerm, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.Authors.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(a =>
                    a.Name.ToLower().Contains(searchTerm) ||
                    a.Email.ToLower().Contains(searchTerm) ||
                    a.Country.ToLower().Contains(searchTerm)
                );
            }
            query = query.OrderBy(a => a.Name);
            return await PaginatedList<Author>.CreateAsync(query, pageIndex, pageSize);
        }

        public async Task<Author> UpdateAuthorAsync(Author author)
        {
            _context.Entry(author).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task DeleteAuthorAsync(string id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
            }
        }
    }
}