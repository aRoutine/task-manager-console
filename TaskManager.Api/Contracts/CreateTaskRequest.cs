using System.ComponentModel.DataAnnotations;
using TaskManager.Models;

namespace TaskManager.Api.Contracts;

public class CreateTaskRequest
{
    [Required(ErrorMessage = "Название задачи обязательно")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Название задачи должно быть от 3 до 100 символов")]
    public string Title { get; set; } = string.Empty;

    [Range(1, 3, ErrorMessage = "Приоритет должен быть в диапазоне от 1 до 3")]
    public TaskPriority Priority { get; set; }

    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Номер страницы должна быть больше 0")]
    public int page { get; set; }

    [Range(minimum: 1, maximum: 100, ErrorMessage = "Размер страниц должен быть от 1 до 100")]
    public int PageSize { get; set; }
}