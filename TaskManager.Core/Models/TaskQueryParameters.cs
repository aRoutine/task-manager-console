using TaskManager.Consts;

namespace TaskManager.Models;

public class TaskQueryParameters
{
    public bool? IsComplete { get; set; }
    public TaskPriority? Priority { get; set; }
    public string? Search { get; set; }
    public int Page { get; set; } = MainConsts.PAGE_DEF;
    public int PageSize { get; set; } = MainConsts.PAGE_SIZE_DEF;
}