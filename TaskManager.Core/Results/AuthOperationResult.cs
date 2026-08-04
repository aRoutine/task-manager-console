namespace TaskManager.Results;

public class AuthOperationResult
{
    public bool Succes { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public int? UserId { get; private set; }

    public static AuthOperationResult Ok(string message, int userId)
    {
        return new AuthOperationResult()
        {
            Succes = true,
            Message = message,
            UserId = userId
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