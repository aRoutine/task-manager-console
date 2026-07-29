using TaskManager.Models;

namespace TaskManager.Api.Contracts;

public class TaskFilterRequest
{
    public bool? IsComplete { get; set; }
    public TaskPriority? Priority { get; set; }
    public string? Search { get; set; }
}