using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BulkkyBook.DTOS.Book
{
    public class BookDto
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ISBN")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN phải từ 10-13 ký tự")]
        public string? ISBN { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập năm xuất bản")]
        [Range(1800, 2100, ErrorMessage = "Năm xuất bản phải từ 1800-2100")]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số trang")]
        [Range(1, 10000, ErrorMessage = "Số trang phải từ 1-10000")]
        public int PageCount { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Range(0, 1000000000, ErrorMessage = "Giá phải từ 0-1.000.000.000")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(0, 1000000, ErrorMessage = "Số lượng phải từ 0-1.000.000")]
        public int StockQuantity { get; set; }
        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn tác giả")]
        public string? AuthorId { get; set; }
        public string? AuthorName { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thể loại")]
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }

    public class CreateBookDto
    {
        [Required(ErrorMessage = "Tên sách không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sách không được quá 200 ký tự")]
        [Display(Name = "Tên sách")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Mô tả không được quá 1000 ký tự")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN không được để trống")]
        [StringLength(20, ErrorMessage = "ISBN không được quá 20 ký tự")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Năm xuất bản không được để trống")]
        [Range(1900, 2100, ErrorMessage = "Năm xuất bản phải từ 1900 đến 2100")]
        [Display(Name = "Năm xuất bản")]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Số trang không được để trống")]
        [Range(1, 10000, ErrorMessage = "Số trang phải từ 1 đến 10000")]
        [Display(Name = "Số trang")]
        public int PageCount { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0, 1000000, ErrorMessage = "Giá phải từ 0 đến 1,000,000")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Đường dẫn ảnh bìa không được quá 500 ký tự")]
        [Display(Name = "Ảnh bìa")]
        public string CoverImage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số lượng tồn kho không được để trống")]
        [Range(0, 100000, ErrorMessage = "Số lượng tồn kho phải từ 0 đến 100,000")]
        [Display(Name = "Số lượng tồn kho")]
        public int StockQuantity { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Tác giả không được để trống")]
        [Display(Name = "Tác giả")]
        public string AuthorId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Thể loại không được để trống")]
        [Display(Name = "Thể loại")]
        public string CategoryId { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }
    }

    public class UpdateBookDto
    {
        [Required(ErrorMessage = "ID không được để trống")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên sách không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sách không được quá 200 ký tự")]
        [Display(Name = "Tên sách")]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Mô tả không được quá 1000 ký tự")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN không được để trống")]
        [StringLength(20, ErrorMessage = "ISBN không được quá 20 ký tự")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Năm xuất bản không được để trống")]
        [Range(1900, 2100, ErrorMessage = "Năm xuất bản phải từ 1900 đến 2100")]
        [Display(Name = "Năm xuất bản")]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Số trang không được để trống")]
        [Range(1, 10000, ErrorMessage = "Số trang phải từ 1 đến 10000")]
        [Display(Name = "Số trang")]
        public int PageCount { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0, 1000000, ErrorMessage = "Giá phải từ 0 đến 1,000,000")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Đường dẫn ảnh bìa không được quá 500 ký tự")]
        [Display(Name = "Ảnh bìa")]
        public string CoverImage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số lượng tồn kho không được để trống")]
        [Range(0, 100000, ErrorMessage = "Số lượng tồn kho phải từ 0 đến 100,000")]
        [Display(Name = "Số lượng tồn kho")]
        public int StockQuantity { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Tác giả không được để trống")]
        [Display(Name = "Tác giả")]
        public string AuthorId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Thể loại không được để trống")]
        [Display(Name = "Thể loại")]
        public string CategoryId { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }
    }
}