using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.UI;

public class ConsoleTaskPrinter : ITaskPrinter
{
    public void PrintTasks(List<TaskItem> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Нет подходящих задач");
            return;
        }

        foreach (TaskItem taskItem in tasks)
        {
            PrintTask(taskItem);
        }

        Console.WriteLine();
        Console.WriteLine("Всего задач: " + tasks.Count);
        Console.WriteLine();
    }

    private void PrintTask(TaskItem task)
    {
        Console.WriteLine($"ID: {task.Id}");
        Console.WriteLine($"Описание: {task.Title}");
        Console.WriteLine($"Приоритет: {task.TaskPriority}");
        Console.WriteLine($"Время создания: {task.CreatedAt}");

        string status = task.IsComplete ? "Выполнено" : "Не выполнено";

        Console.WriteLine($"Статус: {status}");
        Console.WriteLine();
    }
}