using TaskManager.Models;
using TaskManager.Results;

namespace TaskManager.Interfaces;

public interface ITaskRepository
{
    Task<List<TaskItem>> GetAllAsync(int userId);
    Task<TaskItem?> GetByIdAsync(int id, int userId);
    Task<PagedResult<TaskItem>> GetPagedAsync(TaskQueryParameters parameters, int userId);
    void Add(TaskItem task);
    void Update(TaskItem task);
    void Delete(TaskItem task);
    Task SaveChangesAsync();
}