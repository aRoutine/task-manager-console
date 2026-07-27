using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Api.Tests.Fakes;

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