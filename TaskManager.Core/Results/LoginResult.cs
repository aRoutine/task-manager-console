using TaskManager.Models;

namespace TaskManager.Results;

public class LoginResult
{
    public bool Success { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public User? User { get; private set; }
    public string Token { get; set; } = string.Empty;

    public static LoginResult Ok(User user, string token)
    {
        return new LoginResult
        {
            Success = true,
            Message = "Вход успешно выполнен",
            User = user,
            Token = token
        };
    }

    public static LoginResult Fail(string message)
    {
        return new LoginResult
        {
            Success = false,
            Message = message
        };
    }
}