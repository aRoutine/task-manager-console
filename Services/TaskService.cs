using TaskManager.Models;
using TaskManager.Storage;

namespace TaskManager.Services;

public class TaskService
{
    private readonly TaskStorage _taskStorage = new ();
    private readonly List<TaskItem> _tasks;
    private int nextId = 1;

    public TaskService()
    {
        _tasks = _taskStorage.LoadTasks();

        if (_tasks.Count > 0)
        {
            nextId = _tasks.Max(t => t.Id) + 1;
        }
    }

    public void AddTask(string title, TaskPriority taskPriority)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Название задачи не может быть пустым");
            return;
        }

        TaskItem newTaskItem = new TaskItem(nextId, title, taskPriority);
        _tasks.Add(newTaskItem);
        nextId++;

        _taskStorage.SaveTasks(_tasks);

        Console.WriteLine("Задача успешно добавлена!");
    }

    public void ShowTasks(List<TaskItem>? taskItems = null)
    {
        if (taskItems == null)
        {
            taskItems = _tasks;
        }

        if (taskItems.Count == 0)
        {
            Console.WriteLine("Нет подходящих задач");
            return;
        }

        PrintTasksByPriority(TaskPriority.High, taskItems);
        PrintTasksByPriority(TaskPriority.Medium, taskItems);
        PrintTasksByPriority(TaskPriority.Low, taskItems);
    }

    public void ShowCompletedTasks()
    {
        List<TaskItem> completedTasksList = _tasks.Where(t => t.IsComplete).ToList();

        if (completedTasksList.Count == 0)
        {
            Console.WriteLine("Нет заданий по заданному фильтру");
            return;
        }

        ShowTasks(completedTasksList);
    }

    public void ShowNotCompletedTasks()
    {
        List<TaskItem> notCompletedTasksList = _tasks.Where(t => !t.IsComplete).ToList();

        if (notCompletedTasksList.Count == 0)
        {
            Console.WriteLine("Нет заданий по заданному фильтру");
            return;
        }

        ShowTasks(notCompletedTasksList);
    }

    public void CompleteTask(int id)
    {
        TaskItem? taskItem = _tasks.FirstOrDefault(t => t.Id == id);

        if (taskItem == null)
        {
            Console.WriteLine("Задача по заданному Id не была найдена");
            return;
        }

        if (taskItem.IsComplete)
        {
            Console.WriteLine("Эта задача уже выполнена");
            return;
        }

        taskItem.IsComplete = true;
        Console.WriteLine($"Задача {taskItem.Title} успешно выполнена !");

        _taskStorage.SaveTasks(_tasks);
    }

    public void DeleteTask(int id)
    {
        TaskItem? taskItem = _tasks.FirstOrDefault(item => item.Id == id);

        if (taskItem == null)
        {
            Console.WriteLine("Задача по заданному Id не была найдена в базе данных");
            return;
        }

        _tasks.Remove(taskItem);
        Console.WriteLine("Задача с заданным Id успешна удалена");

        _taskStorage.SaveTasks(_tasks);
    }

    public void RenameTask(int id, string title)
    {
        TaskItem? taskItem = _tasks.FirstOrDefault(t => t.Id == id);

        if (taskItem == null)
        {
            Console.WriteLine("Задание по заданному ID не существует");
            return;
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Описание не может быть пустым");
            return;
        }

        taskItem.Title = title;
        Console.WriteLine("Описание задания успешно исправлено !");

        _taskStorage.SaveTasks(_tasks);
    }

    private void PrintTasksByPriority(TaskPriority filter, List<TaskItem> taskItems)
    {
        foreach (TaskItem taskItem in taskItems)
        {
            if (taskItem.TaskPriority != filter)
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