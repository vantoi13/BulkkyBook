using BulkkyBook.Data.Entities;

namespace BulkkyBook.Repositories.Identity;

public interface IUserRepository
{
    ICollection<User> GetUsers();
    User? GetUser(string Id);
    User UpdateUser(User user);
}