namespace TaskManager.Results;

public class TaskOperationResult
{
    public bool Success { get; }
    public string Message { get; }
    public int? TaskId { get; }
    public TaskOperationError Error { get; }

    private TaskOperationResult(bool success, string message, TaskOperationError error = TaskOperationError.None, int? taskId = null)
    {
        Success = success;
        Message = message;
        TaskId = taskId;
        Error = error;
    }

    public static TaskOperationResult Ok(string message, int? taskId = null)
    {
        return new TaskOperationResult(true, message, TaskOperationError.None, taskId);
    }

    public static TaskOperationResult Fail(string message, TaskOperationError error)
    {
        return new TaskOperationResult(false, message, error);
    }
}