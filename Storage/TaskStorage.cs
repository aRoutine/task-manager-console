using System.Text.Json;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Storage;

public class TaskStorage: ITaskStorage
{
    private readonly string _tasksPath = "tasks.json";

    public List<TaskItem> LoadTasks()
    {
        if (!File.Exists(_tasksPath))
        {
            return new List<TaskItem>();
        }

        var json = File.ReadAllText(_tasksPath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<TaskItem>();
        }

        List<TaskItem>? tasks = JsonSerializer.Deserialize<List<TaskItem>>(json);

        return tasks ?? new List<TaskItem>();
    } 

    public void SaveTasks(List<TaskItem> tasks)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(tasks, options);

        File.WriteAllText(_tasksPath, json);
    }
}