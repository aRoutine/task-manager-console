using TaskManager.Models;
using TaskManager.Results;
using TaskManager.Interfaces;

namespace TaskManager.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskOperationResult> AddTaskAsync(string title, TaskPriority taskPriority)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return TaskOperationResult.Fail("Название задачи не может быть пустым");
        }

        TaskItem task = new TaskItem
        {
            Title = title,
            TaskPriority = taskPriority,
            IsComplete = false,
            CreatedAt = DateTime.UtcNow
        };

        _taskRepository.Add(task);

        await _taskRepository.SaveChangesAsync();

        return TaskOperationResult.Ok("Задача успешно добавлена", task.Id);
    }

    public async Task<List<TaskItem>> GetTasksAsync()
    {
        List<TaskItem>? tasks = await _taskRepository.GetAllAsync();

        return tasks
        .OrderByDescending(t => t.TaskPriority)
        .ThenBy(t => t.CreatedAt)
        .ToList();
    }

    public async Task<List<TaskItem>> GetCompletedTasksAsync()
    {
        List<TaskItem>? tasks = await _taskRepository.GetAllAsync();

        return tasks
        .Where(t => t.IsComplete)
        .OrderByDescending(t => t.TaskPriority)
        .ThenBy(t => t.CreatedAt)
        .ToList();
    }

    public async Task<List<TaskItem>> GetNotCompletedTasksAsync()
    {
        List<TaskItem>? tasks = await _taskRepository.GetAllAsync();

        return tasks
        .Where(t => !t.IsComplete)
        .OrderByDescending(t => t.TaskPriority)
        .ThenBy(t => t.CreatedAt)
        .ToList();
    }

    public async Task<List<TaskItem>> GetHighPriorityTasksAsync()
    {
        List<TaskItem>? tasks = await _taskRepository.GetAllAsync();

        return tasks
        .Where(t => t.TaskPriority == TaskPriority.High)
        .OrderBy(t => t.CreatedAt)
        .ToList();
    }

    public async Task<TaskOperationResult> CompleteTaskAsync(int id)
    {
        TaskItem? taskItem = await _taskRepository.GetByIdAsync(id);

        if (taskItem == null)
        {
            return TaskOperationResult.Fail("Задача по заданному ID не найдена");
        }

        if (taskItem.IsComplete)
        {
            return TaskOperationResult.Fail("Задача уже выполнена");
        }

        taskItem.IsComplete = true;

        await _taskRepository.SaveChangesAsync();

        return TaskOperationResult.Ok("Задача успешно выполнена");
    }

    public async Task<TaskOperationResult> DeleteTaskAsync(int id)
    {
        TaskItem? taskItem = await _taskRepository.GetByIdAsync(id);

        if (taskItem == null)
        {
            return TaskOperationResult.Fail("Задача по заданному Id не была найдена в базе данных");
        }

        _taskRepository.Delete(taskItem);

        await _taskRepository.SaveChangesAsync();

        return TaskOperationResult.Ok("Задача успешно удалена");

    }

    public async Task<TaskOperationResult> RenameTaskAsync(int id, string title)
    {
        TaskItem? taskItem = await _taskRepository.GetByIdAsync(id);

        if (taskItem == null)
        {
            return TaskOperationResult.Fail("Задачи по заданному ID не существует");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            return TaskOperationResult.Fail("Задача не может иметь пустое описание");
        }

        taskItem.Title = title;

        _taskRepository.Update(taskItem);

        await _taskRepository.SaveChangesAsync();

        return TaskOperationResult.Ok("Описание успешно изменено");
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        return await _taskRepository.GetByIdAsync(id);
    }
}