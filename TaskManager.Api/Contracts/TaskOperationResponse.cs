namespace TaskManager.Api.Contracts;

public class TaskOperationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? TaskId { get; set; }
}