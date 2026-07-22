using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Contracts;

public class RenameTaskRequest
{
    [Required(ErrorMessage = "Название задачи обязательно")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Название задачи должно быть от 3 до 100 символов")]
    public string Title = string.Empty;
}