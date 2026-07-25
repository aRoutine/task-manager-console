using TaskManager.Models;
using TaskManager.Storage;
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

    public TaskOperationResult AddTask(string title, TaskPriority taskPriority)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return TaskOperationResult.Fail("Название задачи не может быть пустым");
        }

        TaskItem newTaskItem = new TaskItem
        {
            Title = title,
            TaskPriority = taskPriority,
            IsComplete = false,
            CreatedAt = DateTime.UtcNow
        };

        _taskRepository.AddTask(newTaskItem);

        _taskRepository.SaveChanges();

        return TaskOperationResult.Ok("Задача успешно добавлена");
    }

    public List<TaskItem> GetTasks()
    {
        return _taskRepository.GetAll()
        .OrderByDescending(t => t.TaskPriority)
        .ThenBy(t => t.CreatedAt)
        .ToList();
    }

    public List<TaskItem> GetCompletedTasks()
    {
        return _taskRepository.GetAll()
        .Where(t => t.IsComplete)
        .OrderByDescending(t => t.TaskPriority)
        .ThenBy(t => t.CreatedAt)
        .ToList();
    }

    public List<TaskItem> GetNotCompletedTasks()
    {
        return _taskRepository.GetAll()
        .Where(t => !t.IsComplete)
        .OrderByDescending(t => t.TaskPriority)
        .ThenBy(t => t.CreatedAt)
        .ToList();
    }

    public List<TaskItem> GetHighPriorityTasks()
    {
        return _taskRepository.GetAll()
        .Where(t => t.TaskPriority == TaskPriority.High)
        .OrderBy(t => t.CreatedAt)
        .ToList();
    }

    public TaskOperationResult CompleteTask(int id)
    {
        TaskItem? taskItem = _taskRepository.GetById(id);

        if (taskItem == null)
        {
            return TaskOperationResult.Fail("Задача по заданному ID не найдена");
        }

        if (taskItem.IsComplete)
        {
            return TaskOperationResult.Fail("Задача уже выполнена");
        }

        taskItem.IsComplete = true;

        _taskRepository.SaveChanges();

        return TaskOperationResult.Ok("Задача успешно выполнена");
    }

    public TaskOperationResult DeleteTask(int id)
    {
        TaskItem? taskItem = _taskRepository.GetById(id);

        if (taskItem == null)
        {
            return TaskOperationResult.Fail("Задача по заданному Id не была найдена в базе данных");
        }

        _taskRepository.DeleteTask(taskItem);

        _taskRepository.SaveChanges();

        return TaskOperationResult.Ok("Задача успешно удалена");

    }

    public TaskOperationResult RenameTask(int id, string title)
    {
        TaskItem? taskItem = _taskRepository.GetById(id);

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

        _taskRepository.SaveChanges();

        return TaskOperationResult.Ok("Описание успешно изменено");
    }
}