using TaskManager.Services;
using TaskManager.Models;
using TaskManager.Results;
using TaskManager.Interfaces;

namespace TaskManager.UI;

public class TaskConsoleApp
{
    private readonly ITaskService _taskService;
    private readonly ITaskPrinter _taskPrinter;

    public TaskConsoleApp(ITaskService taskService, ITaskPrinter taskPrinter)
    {
        _taskService = taskService;
        _taskPrinter = taskPrinter;
    }

    public async Task Run()
    {
        while (true)
        {
            ShowMainMenu();

            string? input = Console.ReadLine();

            Console.WriteLine();

            switch (input)
            {
                case "1":
                    await AddTask();
                    break;

                case "2":
                    await ShowTasks();
                    break;

                case "3":
                    await DeleteTask();
                    break;

                case "4":
                    await CompleteTask();
                    break;

                case "5":
                    await RenameTask();
                    break;

                case "0":
                    Console.WriteLine("Выход из программы.");
                    return;

                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
            }
        }
    }

    private void ShowMainMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Task Manager ===");
        Console.WriteLine("1. Добавить задачу");
        Console.WriteLine("2. Показать задачи");
        Console.WriteLine("3. Удалить задачу");
        Console.WriteLine("4. Отметить задачу выполненной");
        Console.WriteLine("5. Изменить описание задачи");
        Console.WriteLine("0. Выйти");
        Console.Write("Выберите действие: ");
    }

    private async Task AddTask()
    {
        Console.WriteLine("Пожалуйста, введите краткое описание задачи: ");
        string? title = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Описание не может быть пустым");
            return;
        }

        TaskPriority? priority = ReadPriority();

        if (priority == null)
        {
            Console.WriteLine("Некорректный приоритет");
            return;
        }

        TaskOperationResult addResult = await _taskService.AddTaskAsync(title, priority.Value);
        Console.WriteLine(addResult.Message);
    }

    private async Task ShowTasks()
    {
        Console.WriteLine("Укажите какие задачи вас интересуют: ");
        Console.WriteLine("1. Все задачи");

        string? filterInput = Console.ReadLine();

        if (int.TryParse(filterInput, out int filterNum))
        {
            if (filterNum == 1)
            {
                // List<TaskItem>? tasks = await _taskService.GetTasksAsync();
                // _taskPrinter.PrintTasks(tasks);
                return;
            }

            Console.WriteLine("Введите корректное число !");
            return;
        }
        else
        {
            Console.WriteLine("Вы ввели некорректный запрос");
            return;
        }
    }

    private async Task DeleteTask()
    {
        int? id = GetTaskId();

        if (id == null)
        {
            Console.WriteLine("Некорректный ID.");
            return;
        }

        TaskOperationResult result = await _taskService.DeleteTaskAsync(id.Value);
        Console.WriteLine(result.Message);
    }

    private async Task CompleteTask()
    {
        int? id = GetTaskId();

        if (id == null)
        {
            Console.WriteLine("Некорректный ID.");
            return;
        }

        TaskOperationResult result = await _taskService.CompleteTaskAsync(id.Value);
        Console.WriteLine(result.Message);
    }

    private async Task RenameTask()
    {
        int? id = GetTaskId("Введите ID задачи, которую хотите редактировать: ");

        if (id == null)
        {
            Console.WriteLine("Неверный формат ID");
            return;
        }

        Console.WriteLine("Введите новое описание задачи: ");
        string? newTitle = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newTitle))
        {
            Console.WriteLine("Вы ввели некорректное описание");
            return;
        }

        TaskOperationResult result = await _taskService.RenameTaskAsync(id.Value, newTitle);
        Console.WriteLine(result.Message);
    }

    //helpers

    private TaskPriority? ReadPriority()
    {
        Console.WriteLine("Введите номер приоритета, доступные номера: ");
        Console.WriteLine("1. Низкий приоритет");
        Console.WriteLine("2. Средний приоритет");
        Console.WriteLine("3. Высокий приоритет");

        string? priorityInput = Console.ReadLine();

        if (!int.TryParse(priorityInput, out int priorityNumber) ||
            !Enum.IsDefined(typeof(TaskPriority), priorityNumber))
        {
            return null;
        }

        return (TaskPriority)priorityNumber;
    }

    private int? GetTaskId(string message = "Введите Id задачи: ")
    {
        Console.WriteLine(message);
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int id))
        {
            return id;
        }

        return null;
    }
}