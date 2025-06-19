namespace BulkkyBook.Utils.Identity
{
    public static class Constants
    {
        public static class Roles
        {
            public const string AdminRoleName = "Admin";
            public const string UserRoleName = "User";
            public const string MannagerRoleName = "Manager";
        }

        public static class Functions
        {
            public const string System = "SYSTEM";
            public const string SystemUser = "SYSTEM_USER";
            public const string SystemRole = "SYSTEM_ROLE";
            public const string SystemPermission = "SYSTEM_PERMISSION";
            public const string SystemFunction = "SYSTEM_FUNCTION";

            // Thêm các function mới cho quản lý sách
            public const string SystemAuthor = "SYSTEM_AUTHOR";
            public const string SystemBook = "SYSTEM_BOOK";
            public const string SystemCategory = "SYSTEM_CATEGORY";
        }
        public static class Commands
        {
            public const string Create = "CREATE";
            public const string Read = "READ";
            public const string Update = "UPDATE";
            public const string Delete = "DELETE";
            public const string EXECUTE = "EXECUTE";
        }
    }
}