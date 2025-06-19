using BulkkyBook.DTOS.Book;
using BulkkyBook.Exceptions;
using BulkkyBook.Services.Book;
using BulkkyBook.Security;
using BulkkyBook.Utils;
using BulkkyBook.Utils.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkkyBook.Controllers;

public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly IAuthorService _authorService;
    private readonly ICategoryService _categoryService;

    public BookController(
        IBookService bookService,
        IAuthorService authorService,
        ICategoryService categoryService)
    {
        _bookService = bookService;
        _authorService = authorService;
        _categoryService = categoryService;
    }

    [Permission(Constants.Functions.SystemBook, Constants.Commands.Read)]
    public async Task<IActionResult> Index(string searchTerm, int pageIndex = 1)
    {
        try
        {
            var books = await _bookService.GetAllBooksAsync(searchTerm, pageIndex, 8);
            ViewBag.SearchTerm = searchTerm;
            return View(books);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tải danh sách sách: {ex.Message}";
            return View(new PaginatedList<BookDto>(new List<BookDto>(), 0, 1, 10));
        }
    }

    [Permission(Constants.Functions.SystemBook, Constants.Commands.Create)]
    public async Task<IActionResult> Create()
    {
        try
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Authors = new SelectList(authors, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tải dữ liệu: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [Permission(Constants.Functions.SystemBook, Constants.Commands.Create)]
    public async Task<IActionResult> Create(CreateBookDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var authors = await _authorService.GetAllAuthorsAsync();
                var categories = await _categoryService.GetAllCategoriesAsync();

                ViewBag.Authors = new SelectList(authors, "Id", "Name");
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(createDto);
            }

            var book = await _bookService.CreateBookAsync(createDto);
            TempData["Success"] = $"Tạo sách '{book.Title}' thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException ex)
        {
            TempData["Error"] = ex.Message;
            var authors = await _authorService.GetAllAuthorsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Authors = new SelectList(authors, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(createDto);
        }
        catch (ConflictException ex)
        {
            TempData["Error"] = ex.Message;
            var authors = await _authorService.GetAllAuthorsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Authors = new SelectList(authors, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(createDto);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tạo sách: {ex.Message}";
            var authors = await _authorService.GetAllAuthorsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Authors = new SelectList(authors, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(createDto);
        }
    }

    [Permission(Constants.Functions.SystemBook, Constants.Commands.Update)]
    public async Task<IActionResult> Edit(string id)
    {
        try
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                TempData["Error"] = "Không tìm thấy sách.";
                return RedirectToAction(nameof(Index));
            }

            var updateDto = new UpdateBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                ISBN = book.ISBN,
                PublicationYear = book.PublicationYear,
                PageCount = book.PageCount,
                Price = book.Price,
                StockQuantity = book.StockQuantity,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                CoverImage = book.ImageUrl,
                IsActive = book.IsActive
            };

            var authors = await _authorService.GetAllAuthorsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Authors = new SelectList(authors, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(updateDto);
        }
        catch (NotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tải thông tin sách: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [Permission(Constants.Functions.SystemBook, Constants.Commands.Update)]
    public async Task<IActionResult> Edit(UpdateBookDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var authors = await _authorService.GetAllAuthorsAsync();
                var categories = await _categoryService.GetAllCategoriesAsync();

                ViewBag.Authors = new SelectList(authors, "Id", "Name");
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(updateDto);
            }

            var book = await _bookService.UpdateBookAsync(updateDto);
            TempData["Success"] = $"Cập nhật sách '{book.Title}' thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException ex)
        {
            TempData["Error"] = ex.Message;
            var authors = await _authorService.GetAllAuthorsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Authors = new SelectList(authors, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
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
            var authors = await _authorService.GetAllAuthorsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Authors = new SelectList(authors, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(updateDto);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi cập nhật sách: {ex.Message}";
            var authors = await _authorService.GetAllAuthorsAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();

            ViewBag.Authors = new SelectList(authors, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(updateDto);
        }
    }

    [HttpPost]
    [Permission(Constants.Functions.SystemBook, Constants.Commands.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _bookService.DeleteBookAsync(id);
            TempData["Success"] = "Xóa sách thành công!";
            return RedirectToAction(nameof(Index));
        }
        catch (NotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi xóa sách: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [Permission(Constants.Functions.SystemBook, Constants.Commands.Read)]
    public async Task<IActionResult> Details(string id)
    {
        try
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                TempData["Error"] = "Không tìm thấy sách.";
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }
        catch (NotFoundException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tải thông tin sách: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [Permission(Constants.Functions.SystemBook, Constants.Commands.Read)]
    public async Task<IActionResult> ByAuthor(string authorId)
    {
        try
        {
            var books = await _bookService.GetBooksByAuthorAsync(authorId);
            var author = await _authorService.GetAuthorByIdAsync(authorId);
            ViewBag.AuthorName = author?.Name;
            return View(books);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tải sách theo tác giả: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    [Permission(Constants.Functions.SystemBook, Constants.Commands.Read)]
    public async Task<IActionResult> ByCategory(string categoryId)
    {
        try
        {
            var books = await _bookService.GetBooksByCategoryAsync(categoryId);
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);
            ViewBag.CategoryName = category?.Name;
            return View(books);
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Lỗi khi tải sách theo thể loại: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}