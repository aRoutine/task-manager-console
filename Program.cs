using TaskManager.Interfaces;
using TaskManager.Services;
using TaskManager.UI;

ITaskService taskService = new TaskService();
ITaskPrinter taskPrinter = new ConsoleTaskPrinter();

TaskConsoleApp app = new TaskConsoleApp(taskService, taskPrinter);

app.Run();


