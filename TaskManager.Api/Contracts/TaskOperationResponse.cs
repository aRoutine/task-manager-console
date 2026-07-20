namespace TaskManager.Api.Contracts;

public class TaskOperationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}