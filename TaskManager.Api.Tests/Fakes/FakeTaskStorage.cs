using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Api.Tests.Fakes;

public class FakeTaskStorage : ITaskStorage
{
    private readonly List<TaskItem> _tasks = new List<TaskItem>();

    public List<TaskItem> LoadTasks()
    {
        return _tasks.ToList();
    }

    public void SaveTasks(List<TaskItem> tasks)
    {
        _tasks.Clear();

        _tasks.AddRange(tasks);
    }
}