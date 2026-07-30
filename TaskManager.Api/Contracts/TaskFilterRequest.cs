using System.ComponentModel.DataAnnotations;
using TaskManager.Models;

namespace TaskManager.Api.Contracts;

public class TaskFilterRequest
{
    public bool? IsComplete { get; set; }
    public TaskPriority? Priority { get; set; }
    public string? Search { get; set; }

    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Номер страницы должна быть больше 0")]
    public int Page { get; set; } = 1;

    [Range(minimum: 1, maximum: 100, ErrorMessage = "Размер страниц должен быть от 1 до 100")]
    public int PageSize { get; set; } = 10;
}