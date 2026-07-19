using TaskManager.Models;

namespace TaskManager.Api.Contracts;

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; }
}