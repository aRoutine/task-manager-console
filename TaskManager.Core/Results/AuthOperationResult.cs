namespace TaskManager.Results;

public class AuthOperationResult
{
    public bool Succes { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public int? UserId { get; private set; }
    public string Token { get; private set; } = string.Empty;

    public static AuthOperationResult Ok(string message, int userId, string token)
    {
        return new AuthOperationResult()
        {
            Succes = true,
            Message = message,
            UserId = userId,
            Token = token
        };
    }

    public static AuthOperationResult Fail(string message)
    {
        return new AuthOperationResult()
        {
          Succes = false,
          Message = message  
        };
    }
}