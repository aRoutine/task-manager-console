using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Tests.Fakes;

public class FakeTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = new List<TaskItem>();
    public int SaveCallCount { get; private set; }

    public void AddTask(TaskItem task)
    {
        int nextId = _tasks.Count == 0 
        ? 1 
        : _tasks.Max(t => t.Id) + 1;

        task.Id = nextId;

        _tasks.Add(task);
    }

    public void DeleteTask(TaskItem task)
    {
        _tasks.Remove(task);
    }

    public List<TaskItem> GetAll()
    {
        return _tasks.ToList();
    }

    public TaskItem? GetById(int id)
    {
        return _tasks.FirstOrDefault(t => t.Id == id);
    }

    public void SaveChanges()
    {
        SaveCallCount++;
    }

    public void Update(TaskItem task)
    {
    }
}