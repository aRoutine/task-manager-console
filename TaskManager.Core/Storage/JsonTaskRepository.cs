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

    public List<TaskItem> GetAll()
    {
        return _tasks.ToList();
    }

    public TaskItem? GetById(int id)
    {
        return _tasks.FirstOrDefault(task => task.Id == id);
    }

    public void AddTask(TaskItem task)
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

    public void DeleteTask(TaskItem task)
    {
        _tasks.Remove(task);
    }

    public void SaveChanges()
    {
        _taskStorage.SaveTasks(_tasks);
    }
}