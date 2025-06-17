namespace BulkkyBook.DTOS.Identity
{
    public class PermissionViewModel
    {
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public IList<FunctionViewModel>? Functions { get; set; }
    }



    public class FunctionViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? ParentId { get; set; }
        public bool HasPermission { get; set; } // Indicates if the function has any permissions
        public IList<CommandViewModel>? Commands { get; set; } // List of permissions associated with the function
    }

    public class CommandViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public bool IsSelected { get; set; } // Indicates if the command is checked for permission
    }

}