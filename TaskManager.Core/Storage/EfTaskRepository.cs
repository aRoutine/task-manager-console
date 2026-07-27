using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Storage;

public class EfTaskRepository : ITaskRepository
{
    private readonly TaskManagerDbContext _dbContext;

    public EfTaskRepository(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(TaskItem task)
    {
        _dbContext.Tasks.Add(task);
    }

    public void Delete(TaskItem task)
    {
        _dbContext.Tasks.Remove(task);
    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        return await _dbContext.Tasks
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Update(TaskItem task)
    {
        _dbContext.Tasks.Update(task);
    }
}