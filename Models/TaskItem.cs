using TaskManager.Services;

namespace TaskManager.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsComplete { get; set; }
    public TaskPriority TaskPriority { get; set; }
    public DateTime CreatedAt { get; set; }

    public TaskItem(int id, string title, TaskPriority taskPriority)
    {
        Id = id;
        Title = title;
        IsComplete = false;
        TaskPriority = taskPriority;
        CreatedAt = DateTime.Now;
    }
}