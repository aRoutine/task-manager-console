using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Contracts;

public class LoginRequest
{
    [Required(ErrorMessage = "Необходимо указать email")]
    [EmailAddress(ErrorMessage = "Некорректный адресс электронной почты")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Необходимо указать пароль")]
    public string Password { get; set; } = string.Empty;
}