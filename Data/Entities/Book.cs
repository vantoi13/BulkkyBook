using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace BulkkyBook.Data.Entities
{
    public class Book : BaseEntity
    {
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

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn tác giả")]
        public string? AuthorId { get; set; }
        public Author? Author { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thể loại")]
        public string? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}