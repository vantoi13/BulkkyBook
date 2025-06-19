using System.Reflection.Metadata;
using BulkkyBook.Data.Entities;
using BulkkyBook.Utils.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace BulkkyBook.Data
{
    public class DataInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DataInitializer(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task InitializeAsync()
        {
            // await ClearExistingDataAsync();
            await InitializeRolesAsync();
            await InitializeUsersAsync();
            await InitializeFunctionsAsync();
            await InitializeSampleDataAsync();
        }

        // private async Task ClearExistingDataAsync()
        // {
        //     // Xóa data cũ để có thể seed lại
        //     _context.Permissions.RemoveRange(_context.Permissions);
        //     _context.CommandsInFunctions.RemoveRange(_context.CommandsInFunctions);
        //     _context.Functions.RemoveRange(_context.Functions);
        //     _context.Commands.RemoveRange(_context.Commands);
        //     await _context.SaveChangesAsync();
        // }

        private async Task InitializeRolesAsync()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = Constants.Roles.AdminRoleName,
                    Name = Constants.Roles.AdminRoleName,
                    NormalizedName = Constants.Roles.AdminRoleName.ToUpper()
                });
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = Constants.Roles.UserRoleName,
                    Name = Constants.Roles.UserRoleName,
                    NormalizedName = Constants.Roles.UserRoleName.ToUpper()
                });
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Id = Constants.Roles.MannagerRoleName,
                    Name = Constants.Roles.MannagerRoleName,
                    NormalizedName = Constants.Roles.MannagerRoleName.ToUpper()
                });
            }
        }
        private async Task InitializeUsersAsync()

        {
            if (!_userManager.Users.Any())
            {
                //user1
                var result1 = await _userManager.CreateAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin@gmail.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    Age = 30,
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumber = "0349254962",

                }, "Admin123@");
                if (result1.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("admin@gmail.com");
                    if (user != null)
                    {
                        await _userManager.AddToRoleAsync(user, Constants.Roles.AdminRoleName);
                    }
                }
                //user2
                var result2 = await _userManager.CreateAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "user@gmail.com",
                    FirstName = "User",
                    LastName = "User",
                    Age = 30,
                    Email = "user@gmail.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumber = "0349254962",

                }, "User123@");

                if (result2.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("user@gmail.com");
                    if (user != null)
                    {
                        await _userManager.AddToRoleAsync(user, Constants.Roles.UserRoleName);
                    }
                }
                var result3 = await _userManager.CreateAsync(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "manager@gmail.com",
                    FirstName = "Manager",
                    LastName = "Manager",
                    Age = 30,
                    Email = "manager@gmail.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumber = "0349254962",

                }, "Manager123@");

                if (result3.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("manager@gmail.com");
                    if (user != null)
                    {
                        await _userManager.AddToRoleAsync(user, Constants.Roles.MannagerRoleName);
                    }
                }
            }
        }
        private async Task InitializeFunctionsAsync()
        {
            if (!_context.Functions.Any())
            {
                _context.Functions.AddRange(new List<Function>
                {
                    new Function
                    {
                        Id = Constants.Functions.System,
                        Name = "Quản lý hệ thống",
                        ParentId = null,
                        SortOrder = 1,
                        Icon = "bi bi-gear",
                        Url = "/system"
                    },
                    new Function
                    {
                        Id = Constants.Functions.SystemUser,
                        Name = "Quản lý người dùng",
                        ParentId = Constants.Functions.System,
                        SortOrder = 2,
                        Icon = "bi bi-people",
                        Url = "/system/user"
                    },
                    new Function
                    {
                        Id = Constants.Functions.SystemRole,
                        Name = "Quản lý vai trò",
                        ParentId = Constants.Functions.System,
                        SortOrder = 3,
                        Icon = "bi bi-people",
                        Url = "/system/role"
                    },
                    new Function
                    {
                        Id = Constants.Functions.SystemPermission,
                        Name = "Quản lý quyền hạn",
                        ParentId = Constants.Functions.System,
                        SortOrder = 4,
                        Icon = "bi bi-key",
                        Url = "/system/permission"
                    },
                    new Function
                    {
                        Id = Constants.Functions.SystemFunction,
                        Name = "Quản lý chức năng",
                        ParentId = Constants.Functions.System,
                        SortOrder = 5,
                        Icon = "bi bi-gear",
                        Url = "/system/function"
                    },
                    new Function
                    {
                        Id = Constants.Functions.SystemAuthor,
                        Name = "Quản lý tác giả",
                        ParentId = Constants.Functions.System,
                        SortOrder = 6,
                        Icon = "bi bi-person-badge",
                        Url = "/author"
                    },
                    new Function
                    {
                        Id = Constants.Functions.SystemCategory,
                        Name = "Quản lý thể loại",
                        ParentId = Constants.Functions.System,
                        SortOrder = 7,
                        Icon = "bi bi-tags",
                        Url = "/category"
                    },
                    new Function
                    {
                        Id = Constants.Functions.SystemBook,
                        Name = "Quản lý sách",
                        ParentId = Constants.Functions.System,
                        SortOrder = 8,
                        Icon = "bi bi-book",
                        Url = "/book"
                    },
                });
                await _context.SaveChangesAsync();
            }

            if (!_context.Commands.Any())
            {
                _context.Commands.AddRange(new List<Command>
                {
                    new Command { Id = Constants.Commands.Create, Name = "Tạo" },
                    new Command { Id = Constants.Commands.Read, Name = "Xem" },
                    new Command { Id = Constants.Commands.Update, Name = "Cập nhật" },
                    new Command { Id = Constants.Commands.Delete, Name = "Xóa" },
                    new Command { Id = Constants.Commands.EXECUTE, Name = "Thực thi" },
                });
                await _context.SaveChangesAsync();
            }

            var functions = await _context.Functions.ToListAsync();
            var commands = await _context.Commands.ToListAsync();

            if (!_context.CommandsInFunctions.Any())
            {
                foreach (var function in functions)
                {
                    _context.CommandsInFunctions.AddRange(new List<CommandInFunction>
                    {
                        new CommandInFunction { FunctionId = function.Id, CommandId = Constants.Commands.Create },
                        new CommandInFunction { FunctionId = function.Id, CommandId = Constants.Commands.Read },
                        new CommandInFunction { FunctionId = function.Id, CommandId = Constants.Commands.Update },
                        new CommandInFunction { FunctionId = function.Id, CommandId = Constants.Commands.Delete },
                        new CommandInFunction { FunctionId = function.Id, CommandId = Constants.Commands.EXECUTE },
                    });
                }
                await _context.SaveChangesAsync();
            }

            if (!_context.Permissions.Any())
            {
                var permissions = new List<Permission>();
                foreach (var function in functions)
                {
                    foreach (var command in commands)
                    {
                        permissions.Add(new Permission
                        {
                            FunctionId = function.Id,
                            CommandId = command.Id,
                            RoleId = Constants.Roles.AdminRoleName
                        });
                    }
                }
                _context.Permissions.AddRange(permissions);
                await _context.SaveChangesAsync();
            }
        }

        private async Task InitializeSampleDataAsync()
        {
            // Thêm tác giả mẫu
            var author1 = new Author
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Nguyễn Nhật Ánh",
                Biography = "Tác giả nổi tiếng với nhiều tác phẩm văn học thiếu nhi",
                BirthDate = new DateTime(1955, 5, 7),
                Country = "Việt Nam",
                Email = "nna@example.com",
                IsActive = true
            };

            var author2 = new Author
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Dale Carnegie",
                Biography = "Tác giả nổi tiếng với các sách về phát triển bản thân",
                BirthDate = new DateTime(1888, 11, 24),
                Country = "Mỹ",
                Email = "dc@example.com",
                IsActive = true
            };

            _context.Authors.AddRange(author1, author2);
            await _context.SaveChangesAsync();

            // Thêm thể loại mẫu
            var category1 = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Văn học",
                Description = "Sách văn học",
                DisplayOrder = 1,
                IconClass = "bi bi-book",
                IsActive = true
            };

            var category2 = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Kỹ năng sống",
                Description = "Sách kỹ năng sống",
                DisplayOrder = 2,
                IconClass = "bi bi-person",
                IsActive = true
            };

            _context.Categories.AddRange(category1, category2);
            await _context.SaveChangesAsync();

            // Thêm sách mẫu
            var book1 = new Book
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Cho tôi xin một vé đi tuổi thơ",
                Description = "Một câu chuyện đầy cảm xúc về tuổi thơ",
                ISBN = "9786041082079",
                PublicationYear = 2008,
                PageCount = 208,
                Price = 65000,
                StockQuantity = 100,
                AuthorId = author1.Id,
                CategoryId = category1.Id
            };

            var book2 = new Book
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Đắc Nhân Tâm",
                Description = "Nghệ thuật đối nhân xử thế",
                ISBN = "9786041082080",
                PublicationYear = 1936,
                PageCount = 320,
                Price = 88000,
                StockQuantity = 150,
                AuthorId = author2.Id,
                CategoryId = category2.Id
            };

            _context.Books.AddRange(book1, book2);
            await _context.SaveChangesAsync();
        }
    }
}