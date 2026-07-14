using TaskManager.Models;
using TaskManager.Storage;
using TaskManager.Results;

namespace TaskManager.Services;

public class TaskService
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

        return TaskOperationResult.Ok("Задача успешно доавлена");
    }

    public TaskOperationResult ShowTasks(List<TaskItem>? taskItems = null)
    {
        if (taskItems == null)
        {
            taskItems = _tasks;
        }

        if (taskItems.Count == 0)
        {
            return TaskOperationResult.Fail("Нет подходящих задач");
        }

        PrintTasksByPriority(TaskPriority.High, taskItems);
        PrintTasksByPriority(TaskPriority.Medium, taskItems);
        PrintTasksByPriority(TaskPriority.Low, taskItems);
        return TaskOperationResult.Ok("Задачи успешно выведены");
    }

    public TaskOperationResult ShowCompletedTasks()
    {
        List<TaskItem> completedTasksList = _tasks.Where(t => t.IsComplete).ToList();

        if (completedTasksList.Count == 0)
        {
            return TaskOperationResult.Fail("Нет заданий по заданному фильтру");
        }

        ShowTasks(completedTasksList);
        return TaskOperationResult.Ok("Задачи успешно отфильтрованны");
    }

    public TaskOperationResult ShowNotCompletedTasks()
    {
        List<TaskItem> notCompletedTasksList = _tasks.Where(t => !t.IsComplete).ToList();

        if (notCompletedTasksList.Count == 0)
        {
            return TaskOperationResult.Fail("Нет заданий по заданному фильтру");
        }

        ShowTasks(notCompletedTasksList);
        return TaskOperationResult.Ok("Задачи успешно отфильтрованны");
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

    private void PrintTasksByPriority(TaskPriority priority, List<TaskItem> taskItems)
    {
        foreach (TaskItem taskItem in taskItems)
        {
            if (taskItem.TaskPriority != priority)
            {
                continue;
            }

            Console.WriteLine($"ID: {taskItem.Id}");
            Console.WriteLine($"Задача: {taskItem.Title}");
            Console.WriteLine($"Приоритет: {taskItem.TaskPriority}");
            Console.WriteLine($"Дата создания: {taskItem.CreatedAt}");

            string status = taskItem.IsComplete ? "Выполнено" : "Не выполнена";

            Console.WriteLine($"Статус: {status}");

            Console.WriteLine("");
        }
    }
}