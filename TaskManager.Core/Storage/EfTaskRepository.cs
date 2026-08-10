using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Results;

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

    public async Task<List<TaskItem>> GetAllAsync(int userId)
    {
        return await _dbContext.Tasks
            .Where(t => t.UserId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<PagedResult<TaskItem>> GetPagedAsync(TaskQueryParameters parameters, int userId)
    {
        IQueryable<TaskItem> query = _dbContext.Tasks
        .Where(t => t.UserId == userId)
        .AsNoTracking();

        if (parameters.IsComplete is not null)
        {
            query = query.Where(t => t.IsComplete == parameters.IsComplete);
        }

        if (parameters.Priority is not null)
        {
            query = query.Where(t => t.TaskPriority == parameters.Priority);
        }

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            string search = parameters.Search.ToLower();

            query = query.Where(t => t.Title.ToLower().Contains(search));
        }

        query = query.OrderByDescending(t => t.TaskPriority)
        .ThenBy(t => t.CreatedAt);

        int TotalCount = await query.CountAsync();

        List<TaskItem> items = await query.Skip((parameters.Page - 1) * parameters.PageSize)
        .Take(parameters.PageSize)
        .ToListAsync();

        return new PagedResult<TaskItem>
        {
            Items = items,
            Page = parameters.Page,
            PageSize = parameters.PageSize,
            TotalCount = TotalCount,
            TotalPages = (int)Math.Ceiling(TotalCount / (double)parameters.PageSize)
        };
    }

    public async Task<TaskItem?> GetByIdAsync(int id, int userId)
    {
        return await _dbContext.Tasks
            .Where(t => t.UserId == userId)
            .FirstOrDefaultAsync(t => t.Id == id);
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