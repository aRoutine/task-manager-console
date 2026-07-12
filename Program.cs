using TaskManager.Models;
using TaskManager.Services;

TaskService taskService = new TaskService();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Task Manager ===");
    Console.WriteLine("1. Добавить задачу");
    Console.WriteLine("2. Показать задачи");
    Console.WriteLine("3. Удалить задачу");
    Console.WriteLine("4. Отметить задачу выполненной");
    Console.WriteLine("5. Иземенить описание задачи");
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
            Console.WriteLine("2. Средний приоритет" );
            Console.WriteLine("3. Высокий приоритет");

            string? priorityInput = Console.ReadLine();

            if (!int.TryParse(priorityInput, out int priorityNumber) ||
                !Enum.IsDefined(typeof(TaskPriority), priorityNumber))
            {
                Console.WriteLine("Некорректный приоритет");
                break;
            }

            TaskPriority priority = (TaskPriority)priorityNumber;

            taskService.AddTask(title, priority);
            break;

        case "2":
            taskService.ShowTasks();
            break;

        case "3":
            Console.WriteLine("Введите Id задачи: ");
            string? inputId = Console.ReadLine();

            if (int.TryParse(inputId, out int id))
            {
                taskService.DeleteTask(id);
                break;
            }

            Console.WriteLine("Некорректный ID.");
            break;

        case "4":
            Console.WriteLine("Введите Id задачи: ");
            string? completeInput = Console.ReadLine();

            if (int.TryParse(completeInput, out int completeId))
            {
                taskService.CompleteTask(completeId);
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

                if (newTitle == null)
                {
                    Console.WriteLine("Вы ввели некорректное описание");
                    break;
                }

                taskService.RenameTask(renameId, newTitle);
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
