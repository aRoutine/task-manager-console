namespace TaskManager.Results;

public class TaskOperationResult
{
    public bool Success { get; }
    public string Message { get; }

    private TaskOperationResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static TaskOperationResult Ok(string message)
    {
        return new TaskOperationResult(true, message);
    }

    public static TaskOperationResult Fail(string message)
    {
        return new TaskOperationResult(false, message);
    }
}