using AutoMapper;
using BulkkyBook.Data.Entities;
using BulkkyBook.DTOS.Book;
using BulkkyBook.Exceptions;
using BulkkyBook.Repositories;
using BulkkyBook.Utils;
using BulkkyBook.Utils.Storage;
using BookEntity = BulkkyBook.Data.Entities.Book;
using Microsoft.AspNetCore.Hosting;

namespace BulkkyBook.Services.Book
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper, IStorageService storageService, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storageService = storageService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<BulkkyBook.Utils.PaginatedList<BookDto>> GetAllBooksAsync(string? searchTerm = null, int pageIndex = 1, int pageSize = 10)
        {
            var books = await _unitOfWork.Book.GetAllAsync();
            var query = books.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lower = searchTerm.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(lower)
                    || b.ISBN.ToLower().Contains(lower)
                    || (!string.IsNullOrEmpty(b.Description) && b.Description.ToLower().Contains(lower))
                    || (b.Author != null && b.Author.Name.ToLower().Contains(lower))
                    || (b.Category != null && b.Category.Name.ToLower().Contains(lower))
                );
            }
            query = query.OrderByDescending(b => b.CreatedDate);
            var paged = BulkkyBook.Utils.PaginatedList<BookDto>.Create(query.Select(b => _mapper.Map<BookDto>(b)), pageIndex, pageSize);
            return paged;
        }

        public async Task<BookDto?> GetBookByIdAsync(string id)
        {
            var book = await _unitOfWork.Book.GetByIdWithDetailsAsync(id);
            if (book == null) throw new BusinessException($"Không tìm thấy sách với ID {id}");
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> CreateBookAsync(CreateBookDto createDto)
        {
            if (await _unitOfWork.Book.ExistsByIsbnAsync(createDto.ISBN))
                throw new BusinessException($"Sách với ISBN {createDto.ISBN} đã tồn tại");
            var book = _mapper.Map<BulkkyBook.Data.Entities.Book>(createDto);
            if (createDto.ImageFile != null)
                book.ImageUrl = await _storageService.SaveFileAsync(createDto.ImageFile, "images/books");
            await _unitOfWork.Book.CreateAsync(book);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> UpdateBookAsync(UpdateBookDto updateDto)
        {
            var book = await _unitOfWork.Book.GetByIdAsync(updateDto.Id);
            if (book == null) throw new BusinessException($"Không tìm thấy sách với ID {updateDto.Id}");
            if (updateDto.ISBN != book.ISBN && await _unitOfWork.Book.ExistsByIsbnAsync(updateDto.ISBN))
                throw new BusinessException($"Sách với ISBN {updateDto.ISBN} đã tồn tại");
            if (updateDto.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(book.ImageUrl))
                    await _storageService.DeleteFileAsync(book.ImageUrl);
                book.ImageUrl = await _storageService.SaveFileAsync(updateDto.ImageFile, "images/books");
            }
            _mapper.Map(updateDto, book);
            await _unitOfWork.Book.UpdateAsync(book);
            return _mapper.Map<BookDto>(book);
        }

        public async Task DeleteBookAsync(string id)
        {
            var book = await _unitOfWork.Book.GetByIdAsync(id);
            if (book == null) throw new BusinessException($"Không tìm thấy sách với ID {id}");
            if (!string.IsNullOrEmpty(book.ImageUrl))
                await _storageService.DeleteFileAsync(book.ImageUrl);
            await _unitOfWork.Book.DeleteAsync(id);
        }

        public async Task<bool> BookExistsAsync(string id)
        {
            return await _unitOfWork.Book.ExistsAsync(id);
        }

        public async Task<BulkkyBook.Utils.PaginatedList<BookDto>> GetBooksByAuthorAsync(string authorId, int pageIndex = 1, int pageSize = 10)
        {
            var books = await _unitOfWork.Book.GetByAuthorAsync(authorId);
            var query = books.AsQueryable().OrderByDescending(b => b.CreatedDate);
            return BulkkyBook.Utils.PaginatedList<BookDto>.Create(query.Select(b => _mapper.Map<BookDto>(b)), pageIndex, pageSize);
        }

        public async Task<BulkkyBook.Utils.PaginatedList<BookDto>> GetBooksByCategoryAsync(string categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var books = await _unitOfWork.Book.GetByCategoryAsync(categoryId);
            var query = books.AsQueryable().OrderByDescending(b => b.CreatedDate);
            return BulkkyBook.Utils.PaginatedList<BookDto>.Create(query.Select(b => _mapper.Map<BookDto>(b)), pageIndex, pageSize);
        }

        public async Task<IEnumerable<BookDto>> SearchBooksAsync(string searchTerm)
        {
            var books = await _unitOfWork.Book.SearchAsync(searchTerm);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }


    }
}