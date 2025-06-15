using BulkkyBook.Data.Entities;
using BulkkyBook.Data;
using Microsoft.AspNetCore.Identity;

namespace BulkkyBook.Repositories.Identity;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public ICollection<User> GetUsers()
    {
        return _context.Users.ToList();
    }

    public User? GetUser(string Id)
    {
        return _context.Users.Find(Id);
    }

    public User UpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
        return user;
    }
}
