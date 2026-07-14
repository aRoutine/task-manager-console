using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Results;
using TaskManager.UI;

TaskService taskService = new TaskService();
ConsoleTaskPrinter taskPrinter = new ConsoleTaskPrinter();

while (true)
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

    string? input = Console.ReadLine();

    Console.WriteLine("");

    switch (input)
    {
        case "1":
            Console.WriteLine("Пожалуйста, введите краткое описание задачи: ");
            string? title = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Описание не может быть пустым");
                break;
            }

            Console.WriteLine("Введите номер приоритета, доступные номера: ");
            Console.WriteLine("1. Низкий приоритет");
            Console.WriteLine("2. Средний приоритет");
            Console.WriteLine("3. Высокий приоритет");

            string? priorityInput = Console.ReadLine();

            if (!int.TryParse(priorityInput, out int priorityNumber) ||
                !Enum.IsDefined(typeof(TaskPriority), priorityNumber))
            {
                Console.WriteLine("Некорректный приоритет");
                break;
            }

            TaskPriority priority = (TaskPriority)priorityNumber;

            TaskOperationResult addResult = taskService.AddTask(title, priority);
            Console.WriteLine(addResult.Message);
            break;

        case "2":

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
                    taskPrinter.PrintTasks(taskService.GetTasks());
                    break;
                }

                if (filterNum == 2)
                {
                    taskPrinter.PrintTasks(taskService.GetCompletedTasks());
                    break;
                }

                if (filterNum == 3)
                {
                    taskPrinter.PrintTasks(taskService.GetNotCompletedTasks());
                    break;
                }

                if (filterNum == 4)
                {
                    taskPrinter.PrintTasks(taskService.GetHighPriorityTasks());
                    break;
                }

                Console.WriteLine("Введите корректное число !");
                break;
            }
            else
            {
                Console.WriteLine("Вы ввели некорректный запрос");
                break;
            }

        case "3":
            Console.WriteLine("Введите Id задачи: ");
            string? inputId = Console.ReadLine();

            if (int.TryParse(inputId, out int id))
            {
                TaskOperationResult deleteResult = taskService.DeleteTask(id);
                Console.WriteLine(deleteResult.Message);
                break;
            }

            Console.WriteLine("Некорректный ID.");
            break;

        case "4":
            Console.WriteLine("Введите Id задачи: ");
            string? completeInput = Console.ReadLine();

            if (int.TryParse(completeInput, out int completeId))
            {
                TaskOperationResult completeResult = taskService.CompleteTask(completeId);
                Console.WriteLine(completeResult.Message);
                break;
            }

            Console.WriteLine("Некорректный ID.");
            break;

        case "5":

            Console.WriteLine("Введите ID задачи, которую хотите редактировать: ");

            string? renameInputId = Console.ReadLine();

            if (int.TryParse(renameInputId, out int renameId))
            {
                Console.WriteLine("Введите новое описание задачи: ");

                string? newTitle = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newTitle))
                {
                    Console.WriteLine("Вы ввели некорректное описание");
                    break;
                }

                TaskOperationResult renameResult = taskService.RenameTask(renameId, newTitle);
                Console.WriteLine(renameResult.Message);
            }
            else
            {
                Console.WriteLine("Неверный формат ID");
                break;
            }
            break;

        case "0":
            Console.WriteLine("Выход из программы.");
            return;

        default:
            Console.WriteLine("Неизвестная команда.");
            break;
    }
}
