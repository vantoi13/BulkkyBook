using System.ComponentModel.DataAnnotations;

namespace BulkkyBook.DTOS.Book
{
    public class CategoryDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public string? IconClass { get; set; }
        public bool IsActive { get; set; }
        public int BookCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Tên thể loại là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên thể loại không được vượt quá 200 ký tự")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Thứ tự hiển thị phải lớn hơn 0")]
        public int DisplayOrder { get; set; } = 1;

        [StringLength(50, ErrorMessage = "Icon CSS class không được vượt quá 50 ký tự")]
        public string? IconClass { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateCategoryDto
    {
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "Tên thể loại là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tên thể loại không được vượt quá 200 ký tự")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Thứ tự hiển thị phải lớn hơn 0")]
        public int DisplayOrder { get; set; }

        [StringLength(50, ErrorMessage = "Icon CSS class không được vượt quá 50 ký tự")]
        public string? IconClass { get; set; }

        public bool IsActive { get; set; }
    }
}