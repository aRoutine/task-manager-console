namespace TaskManager.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
    public TaskPriority TaskPriority { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public TaskItem(string title, TaskPriority taskPriority)
    {
        Title = title;
        IsComplete = false;
        TaskPriority = taskPriority;
        CreatedAt = DateTime.UtcNow;
    }

    public TaskItem()
    {

    }
}