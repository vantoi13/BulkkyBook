using AutoMapper;
using BulkkyBook.Data.Entities;
using BulkkyBook.DTOS.Book;
using BulkkyBook.Exceptions;
using BulkkyBook.Repositories;
using BulkkyBook.Utils.Storage;

namespace BulkkyBook.Services.Book
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper, IStorageService storageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
        {
            var authors = await _unitOfWork.Author.GetAllAuthorsAsync();
            var dtos = _mapper.Map<List<AuthorDto>>(authors);
            foreach (var dto in dtos)
            {
                dto.BookCount = await _unitOfWork.Book.GetBookCountByAuthorAsync(dto.Id);
            }
            return dtos;
        }

        public async Task<AuthorDto?> GetAuthorByIdAsync(string id)
        {
            var author = await _unitOfWork.Author.GetAuthorByIdAsync(id);
            if (author != null)
            {
                var authorDto = _mapper.Map<AuthorDto>(author);
                // Nếu cần BookCount, phải lấy từ BookRepository hoặc service khác  
                return authorDto;
            }
            return null;
        }

        public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorDto createDto)
        {
            if (await _unitOfWork.Author.ExistsByNameAsync(createDto.Name))
                throw new ConflictException($"Tên tác giả '{createDto.Name}' đã tồn tại trong hệ thống.");
            if (!string.IsNullOrEmpty(createDto.Email) && await _unitOfWork.Author.ExistsByEmailAsync(createDto.Email))
                throw new ConflictException($"Email '{createDto.Email}' đã tồn tại trong hệ thống.");

            // Xử lý upload file nếu có
            if (createDto.ImageFile != null && createDto.ImageFile.Length > 0)
            {
                createDto.PhotoUrl = await _storageService.SaveFileAsync(createDto.ImageFile, "images/authors");
            }

            var author = _mapper.Map<Author>(createDto);
            author.CreatedDate = DateTime.Now;
            var createdAuthor = await _unitOfWork.Author.CreateAuthorAsync(author);
            return _mapper.Map<AuthorDto>(createdAuthor);
        }

        public async Task<AuthorDto> UpdateAuthorAsync(UpdateAuthorDto updateDto)
        {
            var author = await _unitOfWork.Author.GetAuthorByIdAsync(updateDto.Id);
            if (author == null)
                throw new NotFoundException($"Không tìm thấy tác giả với ID: {updateDto.Id}");
            // Kiểm tra trùng tên (nếu đổi tên)
            if (!string.Equals(author.Name, updateDto.Name, StringComparison.OrdinalIgnoreCase)
                && await _unitOfWork.Author.ExistsByNameAsync(updateDto.Name))
                throw new ConflictException($"Tên tác giả '{updateDto.Name}' đã tồn tại trong hệ thống.");
            // Kiểm tra trùng email (nếu đổi email)
            if (!string.IsNullOrEmpty(updateDto.Email)
                && !string.Equals(author.Email, updateDto.Email, StringComparison.OrdinalIgnoreCase)
                && await _unitOfWork.Author.ExistsByEmailAsync(updateDto.Email))
                throw new ConflictException($"Email '{updateDto.Email}' đã tồn tại trong hệ thống.");

            // Xử lý upload file nếu có
            if (updateDto.ImageFile != null && updateDto.ImageFile.Length > 0)
            {
                updateDto.PhotoUrl = await _storageService.SaveFileAsync(updateDto.ImageFile, "images/authors");
            }

            _mapper.Map(updateDto, author);
            author.UpdatedDate = DateTime.Now;
            var updatedAuthor = await _unitOfWork.Author.UpdateAuthorAsync(author);
            return _mapper.Map<AuthorDto>(updatedAuthor);
        }

        public async Task DeleteAuthorAsync(string id)
        {
            var author = await _unitOfWork.Author.GetAuthorByIdAsync(id);
            if (author == null)
                throw new NotFoundException($"Không tìm thấy tác giả với ID: {id}");
            await _unitOfWork.Author.DeleteAuthorAsync(id);
        }
    }
}