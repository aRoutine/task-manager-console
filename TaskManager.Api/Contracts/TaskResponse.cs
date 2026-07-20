using TaskManager.Models;

namespace TaskManager.Api.Contracts;

public class TaskResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
    public TaskPriority TaskPriority { get; set; }
    public DateTime CreatedAt { get; set; }
}