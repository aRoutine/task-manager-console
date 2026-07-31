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
}