using BulkkyBook.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace  BulkkyBook.DTOS.Identity
{
    public class EditUserViewModels
    {
        public User? User { get; set; } 
        public IList<SelectListItem>? Roles { get; set; }
    }
}