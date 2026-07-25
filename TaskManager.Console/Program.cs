using TaskManager.Interfaces;
using TaskManager.Services;
using TaskManager.Storage;
using TaskManager.UI;

ITaskRepository taskRepository = new JsonTaskRepository();
ITaskService taskService = new TaskService(taskRepository);
ITaskPrinter taskPrinter = new ConsoleTaskPrinter();

TaskConsoleApp app = new TaskConsoleApp(taskService, taskPrinter);

app.Run();


