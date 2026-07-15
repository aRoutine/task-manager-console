using TaskManager.Models;
using TaskManager.Storage;
using TaskManager.Results;
using TaskManager.Interfaces;

namespace TaskManager.Services;

public class TaskService: ITaskService
{
    private readonly TaskStorage _taskStorage = new();
    private readonly List<TaskItem> _tasks;
    private int _nextId = 1;

    public TaskService()
    {
        _tasks = _taskStorage.LoadTasks();

        if (_tasks.Count > 0)
        {
            _nextId = _tasks.Max(t => t.Id) + 1;
        }
    }

    public TaskOperationResult AddTask(string title, TaskPriority taskPriority)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return TaskOperationResult.Fail("Название задачи не может быть пустым");
        }

        TaskItem newTaskItem = new TaskItem(_nextId, title, taskPriority);
        _tasks.Add(newTaskItem);
        _nextId++;

        _taskStorage.SaveTasks(_tasks);

        return TaskOperationResult.Ok("Задача успешно добавлена");
    }

    public List<TaskItem> GetTasks()
    {
        return _tasks
        .OrderByDescending(t => t.TaskPriority)
        .ThenBy(t => t.CreatedAt)
        .ToList();
    }

    public List<TaskItem> GetCompletedTasks()
    {
        return _tasks
        .Where(t => t.IsComplete)
        .OrderByDescending(t => t.TaskPriority)
        .ThenBy(t => t.CreatedAt)
        .ToList();
    }

    public List<TaskItem> GetNotCompletedTasks()
    {
        return _tasks
        .Where(t => !t.IsComplete)
        .OrderByDescending(t => t.TaskPriority)
        .ThenBy(t => t.CreatedAt)
        .ToList();
    }

    public List<TaskItem> GetHighPriorityTasks()
    {
        return _tasks
        .Where(t => t.TaskPriority == TaskPriority.High)
        .OrderBy(t => t.CreatedAt)
        .ToList();
    }

    public TaskOperationResult CompleteTask(int id)
    {
        TaskItem? taskItem = _tasks.FirstOrDefault(t => t.Id == id);

        if (taskItem == null)
        {
            return TaskOperationResult.Fail("Задача по заданному ID не найдена");
        }

        if (taskItem.IsComplete)
        {
            return TaskOperationResult.Fail("Задача уже выполнена");
        }

        taskItem.IsComplete = true;

        _taskStorage.SaveTasks(_tasks);

        return TaskOperationResult.Ok("Задача успешно выполнена");
    }

    public TaskOperationResult DeleteTask(int id)
    {
        TaskItem? taskItem = _tasks.FirstOrDefault(item => item.Id == id);

        if (taskItem == null)
        {
            return TaskOperationResult.Fail("Задача по заданному Id не была найдена в базе данных");
        }

        _tasks.Remove(taskItem);

        _taskStorage.SaveTasks(_tasks);

        return TaskOperationResult.Ok("Задача успешно удалена");

    }

    public TaskOperationResult RenameTask(int id, string title)
    {
        TaskItem? taskItem = _tasks.FirstOrDefault(t => t.Id == id);

        if (taskItem == null)
        {
            return TaskOperationResult.Fail("Задачи по заданному ID не существует");
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            return TaskOperationResult.Fail("Задача не может иметь пустое описание");
        }

        taskItem.Title = title;

        _taskStorage.SaveTasks(_tasks);

        return TaskOperationResult.Ok("Описание успешно изменено");
    }
}