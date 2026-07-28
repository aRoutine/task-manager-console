namespace TaskManager.Results;

public class TaskOperationResult
{
    public bool Success { get; }
    public string Message { get; }
    public int? TaskId { get; }

    private TaskOperationResult(bool success, string message, int? taskId = null)
    {
        Success = success;
        Message = message;
        TaskId = taskId;
    }

    public static TaskOperationResult Ok(string message, int? taskId = null)
    {
        return new TaskOperationResult(true, message, taskId);
    }

    public static TaskOperationResult Fail(string message)
    {
        return new TaskOperationResult(false, message);
    }
}