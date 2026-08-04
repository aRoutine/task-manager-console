using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Contracts;

public class RegisterRequest
{
    [Required(ErrorMessage = "Необходимо ввести обязательное поле - UserName")]
    [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "Имя пользователя должно содержать от 3 до 50 символов")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Необходимо ввести обязательное поле - Email")]
    [EmailAddress(ErrorMessage = "Некорректный адресс электронной почты")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Необходимо ввести обязательное поле - Password")]
    [StringLength(maximumLength: 100, MinimumLength = 8, ErrorMessage = "Пароль должен содержать не менее 8 символов")]
    public string Password { get; set; } = string.Empty;
}