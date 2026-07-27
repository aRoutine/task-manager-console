using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Storage;

public class JsonTaskRepository : ITaskRepository
{ 
    private readonly TaskStorage _taskStorage = new();
    private readonly List<TaskItem> _tasks;

    public JsonTaskRepository()
    {
        _tasks = _taskStorage.LoadTasks();
    }

    public Task<List<TaskItem>> GetAllAsync()
    {
        return Task.FromResult(_tasks.ToList());
    }

    public Task<TaskItem?> GetByIdAsync(int id)
    {
        return Task.FromResult(_tasks.FirstOrDefault(task => task.Id == id));
    }

    public void Add(TaskItem task)
    {
        int nextId = _tasks.Count == 0
            ? 1
            : _tasks.Max(task => task.Id) + 1;

        task.Id = nextId;

        _tasks.Add(task);
    }

    public void Update(TaskItem task)
    {
        
    }

    public void Delete(TaskItem task)
    {
        _tasks.Remove(task);
    }

    public Task SaveChangesAsync()
    {
        _taskStorage.SaveTasks(_tasks);

        return Task.CompletedTask;
    }
}