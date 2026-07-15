using TaskManager.Services;
using TaskManager.Models;
using TaskManager.Results;

namespace TaskManager.UI;

public class TaskConsoleApp
{
    private readonly TaskService _taskService;
    private readonly ConsoleTaskPrinter _taskPrinter;

    public TaskConsoleApp(TaskService taskService, ConsoleTaskPrinter taskPrinter)
    {
        _taskService = taskService;
        _taskPrinter = taskPrinter;
    }

    public void Run()
    {
        while (true)
        {
            ShowMainMenu();

            string? input = Console.ReadLine();

            Console.WriteLine();

            switch (input)
            {
                case "1":
                    AddTask();
                    break;

                case "2":
                    ShowTasks();
                    break;

                case "3":
                    DeleteTask();
                    break;

                case "4":
                    CompleteTask();
                    break;

                case "5":
                    RenameTask();
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

    private void AddTask()
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

        TaskOperationResult addResult = _taskService.AddTask(title, priority.Value);
        Console.WriteLine(addResult.Message);
    }

    private void ShowTasks()
    {
        Console.WriteLine("Укажите какие задачи вас интересуют: ");
        Console.WriteLine("1. Все задачи");
        Console.WriteLine("2. Выполненные задачи");
        Console.WriteLine("3. Невыполненные задачи");
        Console.WriteLine("4. Только задачи с высоким приоритетом");

        string? filterInput = Console.ReadLine();

        if (int.TryParse(filterInput, out int filterNum))
        {
            if (filterNum == 1)
            {
                _taskPrinter.PrintTasks(_taskService.GetTasks());
                return;
            }

            if (filterNum == 2)
            {
                _taskPrinter.PrintTasks(_taskService.GetCompletedTasks());
                return;
            }

            if (filterNum == 3)
            {
                _taskPrinter.PrintTasks(_taskService.GetNotCompletedTasks());
                return;
            }

            if (filterNum == 4)
            {
                _taskPrinter.PrintTasks(_taskService.GetHighPriorityTasks());
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

    private void DeleteTask()
    {
        int? id = GetTaskId();

        if (id == null)
        {
            Console.WriteLine("Некорректный ID.");
            return;
        }

        TaskOperationResult result = _taskService.DeleteTask(id.Value);
        Console.WriteLine(result.Message);
    }

    private void CompleteTask()
    {
        int? id = GetTaskId();

        if (id == null)
        {
            Console.WriteLine("Некорректный ID.");
            return;
        }

        TaskOperationResult result = _taskService.CompleteTask(id.Value);
        Console.WriteLine(result.Message);
    }

    private void RenameTask()
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

        TaskOperationResult result = _taskService.RenameTask(id.Value, newTitle);
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