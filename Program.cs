using TaskManager.models;
using TaskManager.services;

TaskService taskService = new TaskService();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Task Manager ===");
    Console.WriteLine("1. Добавить задачу");
    Console.WriteLine("2. Показать задачи");
    Console.WriteLine("3. Удалить задачу");
    Console.WriteLine("4. Отметить задачу выполненной");
    Console.WriteLine("0. Выйти");
    Console.Write("Выберите действие: ");

    string? input = Console.ReadLine();

    Console.WriteLine("");

    switch (input)
    {
        case "1":
            Console.WriteLine("Пожалуйста, введите краткое описание задачи: ");
            string? title = Console.ReadLine();

            if (string.IsNullOrEmpty(title))
            {
                Console.WriteLine("Описание не может быть пустым");
                break;
            }

            taskService.AddTask(title);
            break;

        case "2":
            if (taskService._tasks.Count == 0)
            {
                Console.WriteLine("У вас отсутствуют какие-либо задачи");
                break;
            }

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

        case "0":
         Console.WriteLine("Выход из программы.");
            return;

        default:
            Console.WriteLine("Неизвестная команда.");
            break;
    }
}
