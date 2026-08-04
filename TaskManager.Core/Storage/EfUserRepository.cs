using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Storage;

public class EfUserRepository : IUserRepository
{
    private readonly TaskManagerDbContext _dbContext;

    public EfUserRepository(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(User user)
    {
        _dbContext.Users.Add(user);
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public Task<User?> GetByUserNameAsync(string userName)
    {
        return _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName);

    }

    public void SaveChangesAsync()
    {
        _dbContext.SaveChangesAsync();
    }
}