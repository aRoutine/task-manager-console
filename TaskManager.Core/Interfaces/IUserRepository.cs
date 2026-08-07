using TaskManager.Models;

namespace TaskManager.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email); 
    Task<User?> GetByUserNameAsync(string userName); 
    void Add(User user);
    Task SaveChangesAsync();
}