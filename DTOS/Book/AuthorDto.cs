using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BulkkyBook.DTOS.Book
{
    public class AuthorDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public string Country { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int BookCount { get; set; }
    }

    public class CreateAuthorDto
    {
        [Required(ErrorMessage = "Tên tác giả không được để trống")]
        [StringLength(100, ErrorMessage = "Tên tác giả không được quá 100 ký tự")]
        [Display(Name = "Tên tác giả")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Tiểu sử không được quá 1000 ký tự")]
        [Display(Name = "Tiểu sử")]
        public string Biography { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime? BirthDate { get; set; }

        [StringLength(50, ErrorMessage = "Quốc gia không được quá 50 ký tự")]
        [Display(Name = "Quốc gia")]
        public string Country { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được quá 100 ký tự")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "URL ảnh không được quá 500 ký tự")]
        [Display(Name = "Ảnh đại diện")]
        public string PhotoUrl { get; set; } = string.Empty;

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Ảnh đại diện (file)")]
        public IFormFile? ImageFile { get; set; }
    }

    public class UpdateAuthorDto
    {
        [Required(ErrorMessage = "ID không được để trống")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên tác giả không được để trống")]
        [StringLength(100, ErrorMessage = "Tên tác giả không được quá 100 ký tự")]
        [Display(Name = "Tên tác giả")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Tiểu sử không được quá 1000 ký tự")]
        [Display(Name = "Tiểu sử")]
        public string Biography { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime? BirthDate { get; set; }

        [StringLength(50, ErrorMessage = "Quốc gia không được quá 50 ký tự")]
        [Display(Name = "Quốc gia")]
        public string Country { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được quá 100 ký tự")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "URL ảnh không được quá 500 ký tự")]
        [Display(Name = "Ảnh đại diện")]
        public string PhotoUrl { get; set; } = string.Empty;

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }

        [Display(Name = "Ảnh đại diện (file)")]
        public IFormFile? ImageFile { get; set; }
    }
}