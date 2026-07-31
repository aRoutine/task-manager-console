using TaskManager.Models;
using TaskManager.Results;

namespace TaskManager.Interfaces;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(int id);
    Task<PagedResult<TaskItem>> GetPagedAsync(TaskQueryParameters parameters);
    void Add(TaskItem task);
    void Update(TaskItem task);
    void Delete(TaskItem task);
    Task SaveChangesAsync();
}