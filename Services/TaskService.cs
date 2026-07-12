using TaskManager.Models;

namespace TaskManager.Services;

public class TaskService
{
    private readonly List<TaskItem> _tasks = new();
    private int nextId = 1;

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

        Console.WriteLine("Задача успешно добавлена!");
    }

    public void ShowTasks()
    {
        if (_tasks.Count == 0)
        {
            Console.WriteLine("В данный момент у вас нет никаких задач");
            return;
        }

        FilterTasks(TaskPriority.High);
        FilterTasks(TaskPriority.Medium);
        FilterTasks(TaskPriority.Low);
    }

    public void CompleteTask(int id)
    {
        TaskItem? taskItem = _tasks.FirstOrDefault(t => t.Id == id);

        if (taskItem == null)
        {
            Console.WriteLine("Задача по заданному Id не была найдена");
            return;
        }

        taskItem.IsComplete = true;
        Console.WriteLine($"Задача {taskItem.Title} успешно выполнена !");
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
    }

    private void FilterTasks(TaskPriority filter)
    {
        foreach (TaskItem taskItem in _tasks)
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