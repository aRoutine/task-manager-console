using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Results;

namespace TaskManager.Tests.Fakes;

public class FakeTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = new List<TaskItem>();
    public int SaveCallCount { get; private set; }

    public void Add(TaskItem task)
    {
        int nextId = _tasks.Count == 0
        ? 1
        : _tasks.Max(t => t.Id) + 1;

        task.Id = nextId;

        _tasks.Add(task);
    }

    public void Delete(TaskItem task)
    {
        _tasks.Remove(task);
    }

    public Task<PagedResult<TaskItem>> GetPagedAsync(TaskQueryParameters parameters)
    {
        IEnumerable<TaskItem> query = _tasks;

        if (parameters.IsComplete is not null)
        {
            query = query.Where(task => task.IsComplete == parameters.IsComplete.Value);
        }

        if (parameters.Priority is not null)
        {
            query = query.Where(task => task.TaskPriority == parameters.Priority.Value);
        }

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            query = query.Where(task =>
                task.Title.Contains(parameters.Search, StringComparison.OrdinalIgnoreCase));
        }

        query = query
            .OrderByDescending(task => task.TaskPriority)
            .ThenBy(task => task.CreatedAt);

        int totalCount = query.Count();

        List<TaskItem> items = query
            .Skip((parameters.Page - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToList();

        PagedResult<TaskItem> result = new PagedResult<TaskItem>
        {
            Items = items,
            Page = parameters.Page,
            PageSize = parameters.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize)
        };

        return Task.FromResult(result);
    }

    public Task<List<TaskItem>> GetAllAsync()
    {
        return Task.FromResult(_tasks.ToList());
    }

    public Task<TaskItem?> GetByIdAsync(int id)
    {
        return Task.FromResult(_tasks.FirstOrDefault(t => t.Id == id));
    }

    public Task SaveChangesAsync()
    {
        SaveCallCount++;

        return Task.CompletedTask;
    }

    public void Update(TaskItem task)
    {
    }
}