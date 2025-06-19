using BulkkyBook.DTOS.Book;
using BulkkyBook.Exceptions;
using BulkkyBook.Services.Book;
using BulkkyBook.Security;
using BulkkyBook.Utils.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkkyBook.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [Permission(Constants.Functions.SystemCategory, Constants.Commands.Read)]
    public async Task<IActionResult> Index(string searchTerm)
    {
        try
        {
            var categories = await _categoryService.SearchCategoriesAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View(categories);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tải danh sách thể loại: {ex.Message}";
            return View(new List<CategoryDto>());
        }
    }

    [Permission(Constants.Functions.SystemCategory, Constants.Commands.Create)]
    public async Task<IActionResult> Create()
    {
        try
        {
            var parentCategories = await _categoryService.GetParentCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(parentCategories, "Id", "Name");
            return View();
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tải danh sách thể loại cha: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [Permission(Constants.Functions.SystemCategory, Constants.Commands.Create)]
    public async Task<IActionResult> Create(CreateCategoryDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var parentCategories = await _categoryService.GetParentCategoriesAsync();
                ViewBag.ParentCategories = new SelectList(parentCategories, "Id", "Name");
                return View(createDto);
            }

            var category = await _categoryService.CreateCategoryAsync(createDto);
            TempData["Success"] = $"Tạo thể loại '{category.Name}' thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException ex)
        {
            TempData["Error"] = ex.Message;
            var parentCategories = await _categoryService.GetParentCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(parentCategories, "Id", "Name");
            return View(createDto);
        }
        catch (ConflictException ex)
        {
            TempData["Error"] = ex.Message;
            var parentCategories = await _categoryService.GetParentCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(parentCategories, "Id", "Name");
            return View(createDto);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tạo thể loại: {ex.Message}";
            var parentCategories = await _categoryService.GetParentCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(parentCategories, "Id", "Name");
            return View(createDto);
        }
    }

    [Permission(Constants.Functions.SystemCategory, Constants.Commands.Update)]
    public async Task<IActionResult> Edit(string id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                TempData["Error"] = "Không tìm thấy thể loại.";
                return RedirectToAction(nameof(Index));
            }

            var updateDto = new UpdateCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                DisplayOrder = category.DisplayOrder,
                IconClass = category.IconClass,
                IsActive = category.IsActive
            };

            var parentCategories = await _categoryService.GetParentCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(parentCategories, "Id", "Name");
            return View(updateDto);
        }
        catch (NotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tải thông tin thể loại: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [Permission(Constants.Functions.SystemCategory, Constants.Commands.Update)]
    public async Task<IActionResult> Edit(UpdateCategoryDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var parentCategories = await _categoryService.GetParentCategoriesAsync();
                ViewBag.ParentCategories = new SelectList(parentCategories, "Id", "Name");
                return View(updateDto);
            }

            var category = await _categoryService.UpdateCategoryAsync(updateDto);
            TempData["Success"] = $"Cập nhật thể loại '{category.Name}' thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException ex)
        {
            TempData["Error"] = ex.Message;
            var parentCategories = await _categoryService.GetParentCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(parentCategories, "Id", "Name");
            return View(updateDto);
        }
        catch (NotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (ConflictException ex)
        {
            TempData["Error"] = ex.Message;
            var parentCategories = await _categoryService.GetParentCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(parentCategories, "Id", "Name");
            return View(updateDto);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi cập nhật thể loại: {ex.Message}";
            var parentCategories = await _categoryService.GetParentCategoriesAsync();
            ViewBag.ParentCategories = new SelectList(parentCategories, "Id", "Name");
            return View(updateDto);
        }
    }

    [HttpPost]
    [Permission(Constants.Functions.SystemCategory, Constants.Commands.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _categoryService.DeleteCategoryAsync(id);
            TempData["Success"] = "Xóa thể loại thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (NotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (ConflictException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi xóa thể loại: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [Permission(Constants.Functions.SystemCategory, Constants.Commands.Read)]
    public async Task<IActionResult> Details(string id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                TempData["Error"] = "Không tìm thấy thể loại.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }
        catch (NotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tải thông tin thể loại: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}