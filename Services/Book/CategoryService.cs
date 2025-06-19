using AutoMapper;
using BulkkyBook.Data.Entities;
using BulkkyBook.DTOS.Book;
using BulkkyBook.Exceptions;
using BulkkyBook.Repositories;

namespace BulkkyBook.Services.Book
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Category.GetAllAsync();
            var dtos = _mapper.Map<List<CategoryDto>>(categories);

            // Lấy toàn bộ sách, group theo CategoryId
            var books = await _unitOfWork.Book.GetAllAsync();
            var bookCountByCategory = books
                .GroupBy(b => b.CategoryId)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var dto in dtos)
            {
                dto.BookCount = bookCountByCategory.ContainsKey(dto.Id) ? bookCountByCategory[dto.Id] : 0;
            }
            return dtos;
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(string id)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(id);
            if (category != null)
            {
                var categoryDto = _mapper.Map<CategoryDto>(category);
                categoryDto.BookCount = await _unitOfWork.Category.GetBookCountAsync(id);
                return categoryDto;
            }
            return null;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto)
        {
            // Check if name already exists
            if (await _unitOfWork.Category.ExistsByNameAsync(createDto.Name))
            {
                throw new ConflictException($"Tên thể loại '{createDto.Name}' đã tồn tại trong hệ thống.");
            }

            // Create new category
            var category = _mapper.Map<Category>(createDto);
            category.CreatedDate = DateTime.Now;

            var createdCategory = await _unitOfWork.Category.CreateAsync(category);
            return _mapper.Map<CategoryDto>(createdCategory);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto updateDto)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(updateDto.Id);
            if (category == null)
            {
                throw new NotFoundException($"Không tìm thấy thể loại với ID: {updateDto.Id}");
            }

            // Check if name already exists (excluding current category)
            if (await _unitOfWork.Category.ExistsByNameAsync(updateDto.Name, updateDto.Id))
            {
                throw new ConflictException($"Tên thể loại '{updateDto.Name}' đã tồn tại trong hệ thống.");
            }

            // Update category
            _mapper.Map(updateDto, category);
            category.UpdatedDate = DateTime.Now;

            var updatedCategory = await _unitOfWork.Category.UpdateAsync(category);
            return _mapper.Map<CategoryDto>(updatedCategory);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            var category = await _unitOfWork.Category.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"Không tìm thấy thể loại với ID: {id}");
            }

            // Check if category has books
            var bookCount = await _unitOfWork.Category.GetBookCountAsync(id);
            if (bookCount > 0)
            {
                throw new ValidationException($"Không thể xóa thể loại '{category.Name}' vì đang có {bookCount} sách.");
            }

            await _unitOfWork.Category.DeleteAsync(id);
        }

        public async Task<bool> CategoryExistsAsync(string id)
        {
            return await _unitOfWork.Category.ExistsAsync(id);
        }

        public async Task<bool> CategoryHasBooksAsync(string id)
        {
            var bookCount = await _unitOfWork.Category.GetBookCountAsync(id);
            return bookCount > 0;
        }

        public async Task<IEnumerable<CategoryDto>> SearchCategoriesAsync(string searchTerm)
        {
            var categories = await _unitOfWork.Category.SearchAsync(searchTerm);
            var dtos = _mapper.Map<List<CategoryDto>>(categories);

            // Lấy toàn bộ sách, group theo CategoryId
            var books = await _unitOfWork.Book.GetAllAsync();
            var bookCountByCategory = books
                .GroupBy(b => b.CategoryId)
                .ToDictionary(g => g.Key, g => g.Count());

            foreach (var dto in dtos)
            {
                dto.BookCount = bookCountByCategory.ContainsKey(dto.Id) ? bookCountByCategory[dto.Id] : 0;
            }
            return dtos;
        }

        public async Task<bool> CategoryHasChildrenAsync(string id)
        {
            // Since categories don't have hierarchical structure, always return false
            return false;
        }

        public async Task<IEnumerable<CategoryDto>> GetParentCategoriesAsync()
        {
            // Since categories don't have hierarchical structure, return all active categories
            var categories = await _unitOfWork.Category.GetAllAsync();
            var activeCategories = categories.Where(c => c.IsActive);
            return _mapper.Map<IEnumerable<CategoryDto>>(activeCategories);
        }
    }
}