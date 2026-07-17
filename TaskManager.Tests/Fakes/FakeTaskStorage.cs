using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Tests.Fakes;

public class FakeTaskStorage : ITaskStorage
{
    private readonly List<TaskItem> _tasks = new List<TaskItem>();

    public int SaveCallCount { get; private set; }

    public List<TaskItem> LoadTasks()
    {
        return _tasks.ToList();
    }

    public void SaveTasks(List<TaskItem> tasks)
    {
        SaveCallCount++;

        _tasks.Clear();
        _tasks.AddRange(tasks);
    }
}