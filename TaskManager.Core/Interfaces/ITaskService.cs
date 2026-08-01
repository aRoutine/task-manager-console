using TaskManager.Results;
using TaskManager.Models;

namespace TaskManager.Interfaces;

public interface ITaskService
{
    Task<TaskOperationResult> AddTaskAsync(string title, TaskPriority taskPriority);

    Task<TaskItem?> GetTaskByIdAsync(int id);

    Task<List<TaskItem>> GetTasksAsync(bool? isComplete, TaskPriority? priority, string? search);

    Task<PagedResult<TaskItem>> GetPagedTasksAsync(TaskQueryParameters parameters);

    Task<List<TaskItem>> GetTasksAsync();

    Task<List<TaskItem>> GetCompletedTasksAsync();

    Task<List<TaskItem>> GetNotCompletedTasksAsync();

    Task<List<TaskItem>> GetHighPriorityTasksAsync();

    Task<TaskOperationResult> UpdateTaskAsync(
        int id,
        string title,
        TaskPriority priority,
        bool isComplete
    );

    Task<TaskOperationResult> CompleteTaskAsync(int id);

    Task<TaskOperationResult> DeleteTaskAsync(int id);

    Task<TaskOperationResult> RenameTaskAsync(int id, string title);
}