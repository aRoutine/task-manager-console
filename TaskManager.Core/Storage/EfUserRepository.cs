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
        _dbContext.Users.AddAsync(user);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName);

    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}