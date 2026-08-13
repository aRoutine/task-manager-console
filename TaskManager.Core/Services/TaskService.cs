using TaskManager.Models;
using TaskManager.Results;
using TaskManager.Interfaces;

namespace TaskManager.Services;

public class TaskService(ITaskRepository _taskRepository, ICurrentUserService _currentUserService) : ITaskService
{
    public async Task<TaskOperationResult> AddTaskAsync(string title, TaskPriority taskPriority)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return TaskOperationResult.Fail("Название задачи не может быть пустым", TaskOperationError.Validation);
        }

        int userId = _currentUserService.UserId;

        TaskItem task = new TaskItem
        {
            Title = title,
            TaskPriority = taskPriority,
            IsComplete = false,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        _taskRepository.Add(task);

        await _taskRepository.SaveChangesAsync();

        return TaskOperationResult.Ok("Задача успешно добавлена", task.Id);
    }

    public async Task<PagedResult<TaskItem>> GetPagedTasksAsync(TaskQueryParameters parameters)
    {
        int userId = _currentUserService.UserId;

        return await _taskRepository.GetPagedAsync(parameters, userId);
    }

    public async Task<TaskOperationResult> CompleteTaskAsync(int id)
    {
        int userId = _currentUserService.UserId;

        TaskItem? taskItem = await _taskRepository.GetByIdAsync(id, userId);

        if (taskItem == null)
        {
            return TaskOperationResult.Fail("Задача по заданному ID не найдена", TaskOperationError.NotFound);
        }

        if (taskItem.IsComplete)
        {
            return TaskOperationResult.Fail("Задача уже выполнена", TaskOperationError.Conflict);
        }

        taskItem.IsComplete = true;

        await _taskRepository.SaveChangesAsync();

        return TaskOperationResult.Ok("Задача успешно выполнена");
    }

    public async Task<TaskOperationResult> DeleteTaskAsync(int id)
    {
        int userId = _currentUserService.UserId;

        TaskItem? taskItem = await _taskRepository.GetByIdAsync(id, userId);

        if (taskItem == null)
        {
            return TaskOperationResult.Fail("Задача по заданному Id не была найдена в базе данных", TaskOperationError.NotFound);
        }

        _taskRepository.Delete(taskItem);

        await _taskRepository.SaveChangesAsync();

        return TaskOperationResult.Ok("Задача успешно удалена");

    }

    public async Task<TaskOperationResult> RenameTaskAsync(int id, string title)
    {
        int userId = _currentUserService.UserId;

        TaskItem? taskItem = await _taskRepository.GetByIdAsync(id, userId);

        if (taskItem == null)
        {
            return TaskOperationResult.Fail("Задачи по заданному ID не существует", TaskOperationError.NotFound);
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            return TaskOperationResult.Fail("Задача не может иметь пустое описание", TaskOperationError.Validation);
        }

        taskItem.Title = title;

        _taskRepository.Update(taskItem);

        await _taskRepository.SaveChangesAsync();

        return TaskOperationResult.Ok("Описание успешно изменено");
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        int userId = _currentUserService.UserId;

        return await _taskRepository.GetByIdAsync(id, userId);
    }

    public async Task<TaskOperationResult> UpdateTaskAsync(int id, string title, TaskPriority priority, bool isComplete)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return TaskOperationResult.Fail("Описание задачи не может быть пустым", TaskOperationError.Validation);
        }

        int userId = _currentUserService.UserId;

        TaskItem? task = await _taskRepository.GetByIdAsync(id, userId);

        if (task == null)
        {
            return TaskOperationResult.Fail("Задача по заданному id не найдена", TaskOperationError.NotFound);
        }

        task.IsComplete = isComplete;
        task.TaskPriority = priority;
        task.Title = title;

        _taskRepository.Update(task);

        await _taskRepository.SaveChangesAsync();

        return TaskOperationResult.Ok("Задача успешно переименованна", task.Id);
    }
}