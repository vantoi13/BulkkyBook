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
            await InitializeRolesAsync();
            await InitializeUsersAsync();
            await InitializeFunctionsAsync();
        }

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
                        Id = "SYSTEM",
                        Name = "Quản lý hệ thống",
                        ParentId = null,
                        SortOrder = 1,
                        Icon = "bi bi-gear",
                        Url = "/system"
                    },
                    new Function
                    {
                        Id = "SYSTEM_USER",
                        Name = "Quản lý người dùng",
                        ParentId = "SYSTEM",
                        SortOrder = 2,
                        Icon = "bi bi-people",
                        Url = "/system/user"
                    },
                    new Function
                    {
                        Id = "SYSTEM_ROLE",
                        Name = "Quản lý vai trò",
                        ParentId = "SYSTEM",
                        SortOrder = 3,
                        Icon = "bi bi-people",
                        Url = "/system/role"
                    },
                    new Function
                    {
                        Id = "SYSTEM_PERMISSION",
                        Name = "Quản lý quyền hạn",
                        ParentId = "SYSTEM",
                        SortOrder = 4,
                        Icon = "bi bi-key",
                        Url = "/system/permission"
                    },
                    new Function
                    {
                        Id = "SYSTEM_FUNCTION",
                        Name = "Quản lý chức năng",
                        ParentId = "SYSTEM",
                        SortOrder = 5,
                        Icon = "bi bi-gear",
                        Url = "/system/function"
                    },
                });
                await _context.SaveChangesAsync();
            }
            if (!_context.Commands.Any())
            {
                _context.Commands.AddRange(new List<Command>
                {
                    new Command
                    {
                        Id = "CREATE",
                        Name = "Tạo"
                    },
                    new Command
                    {
                        Id = "READ",
                        Name = "Xem"
                    },
                    new Command
                    {
                        Id = "UPDATE",
                        Name = "Cập nhật"
                    },
                    new Command
                    {
                        Id = "DELETE",
                        Name = "Xóa"
                    },
                    new Command
                    {
                        Id = "EXECUTE",
                        Name = "Thực thi"
                    },
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
            new CommandInFunction { FunctionId = function.Id, CommandId = "CREATE" },
            new CommandInFunction { FunctionId = function.Id, CommandId = "READ" },
            new CommandInFunction { FunctionId = function.Id, CommandId = "UPDATE" },
            new CommandInFunction { FunctionId = function.Id, CommandId = "DELETE" },
            new CommandInFunction { FunctionId = function.Id, CommandId = "EXECUTE" },
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
    }
}