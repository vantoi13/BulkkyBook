using System.Reflection.Metadata;
using BulkkyBook.Data.Entities;
using BulkkyBook.Utils.Identity;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace BulkkyBook.Data
{
    public class DataInitializer
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DataInitializer(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task Seed()
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
    }
}