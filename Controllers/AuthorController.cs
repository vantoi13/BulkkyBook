using BulkkyBook.DTOS.Book;
using BulkkyBook.Exceptions;
using BulkkyBook.Services.Book;
using BulkkyBook.Security;
using BulkkyBook.Utils;
using BulkkyBook.Utils.Identity;
using BulkkyBook.Utils.Storage;
using Microsoft.AspNetCore.Mvc;

namespace BulkkyBook.Controllers;

public class AuthorController : Controller
{
    private readonly IAuthorService _authorService;
    private readonly IStorageService _storageService;

    public AuthorController(IAuthorService authorService, IStorageService storageService)
    {
        _authorService = authorService;
        _storageService = storageService;
    }

    [Permission(Constants.Functions.SystemAuthor, Constants.Commands.Read)]
    public async Task<IActionResult> Index(string? searchTerm, string? status, int pageIndex = 1)
    {
        try
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            var filteredAuthors = authors.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
                filteredAuthors = filteredAuthors.Where(a => a.Name != null && a.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(status))
            {
                bool isActive = status == "true";
                filteredAuthors = filteredAuthors.Where(a => a.IsActive == isActive);
            }

            var pagedAuthors = PaginatedList<AuthorDto>.Create(filteredAuthors, pageIndex, 5);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.Status = status;
            return View(pagedAuthors);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tải danh sách tác giả: {ex.Message}";
            return View(new PaginatedList<AuthorDto>(new List<AuthorDto>(), 0, 1, 5));
        }
    }

    [Permission(Constants.Functions.SystemAuthor, Constants.Commands.Create)]
    public IActionResult Create()
    {
        return View(new CreateAuthorDto());
    }

    [HttpPost]
    [Permission(Constants.Functions.SystemAuthor, Constants.Commands.Create)]
    public async Task<IActionResult> Create(CreateAuthorDto authorDto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (authorDto.ImageFile != null && authorDto.ImageFile.Length > 0)
                {
                    authorDto.PhotoUrl = await _storageService.SaveFileAsync(authorDto.ImageFile, "images/authors");
                }
                await _authorService.CreateAuthorAsync(authorDto);
                TempData["Success"] = "Thêm tác giả thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (ConflictException ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm tác giả.");
                TempData["Error"] = ex.Message;
            }
        }
        return View(authorDto);
    }

    [Permission(Constants.Functions.SystemAuthor, Constants.Commands.Update)]
    public async Task<IActionResult> Edit(string id)
    {
        try
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                TempData["Error"] = "Không tìm thấy tác giả.";
                return RedirectToAction(nameof(Index));
            }

            var updateDto = new UpdateAuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                BirthDate = author.BirthDate,
                Country = author.Country,
                Email = author.Email,
                PhotoUrl = author.PhotoUrl,
                IsActive = author.IsActive
            };

            return View(updateDto);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Có lỗi xảy ra khi tải thông tin tác giả: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [Permission(Constants.Functions.SystemAuthor, Constants.Commands.Update)]
    public async Task<IActionResult> Edit(string id, UpdateAuthorDto authorDto)
    {
        if (id != authorDto.Id)
        {
            TempData["Error"] = "ID tác giả không hợp lệ.";
            return RedirectToAction(nameof(Index));
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (authorDto.ImageFile != null && authorDto.ImageFile.Length > 0)
                {
                    authorDto.PhotoUrl = await _storageService.SaveFileAsync(authorDto.ImageFile, "images/authors");
                }
                await _authorService.UpdateAuthorAsync(authorDto);
                TempData["Success"] = "Cập nhật tác giả thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (ConflictException ex)
            {
                ModelState.AddModelError("", ex.Message);
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật tác giả.");
                TempData["Error"] = ex.Message;
            }
        }
        return View(authorDto);
    }

    [HttpPost]
    [Permission(Constants.Functions.SystemAuthor, Constants.Commands.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _authorService.DeleteAuthorAsync(id);
            TempData["Success"] = "Xóa tác giả thành công!";
        }
        catch (NotFoundException ex)
        {
            TempData["Error"] = ex.Message;
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Có lỗi xảy ra khi xóa tác giả: {ex.Message}";
        }
        return RedirectToAction(nameof(Index));
    }

    [Permission(Constants.Functions.SystemAuthor, Constants.Commands.Read)]
    public async Task<IActionResult> Details(string id)
    {
        try
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                TempData["Error"] = "Không tìm thấy tác giả.";
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Có lỗi xảy ra khi tải thông tin tác giả: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}