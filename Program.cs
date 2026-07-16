using TaskManager.Interfaces;
using TaskManager.Services;
using TaskManager.Storage;
using TaskManager.UI;

ITaskStorage taskStorage = new TaskStorage();
ITaskService taskService = new TaskService(taskStorage);
ITaskPrinter taskPrinter = new ConsoleTaskPrinter();

TaskConsoleApp app = new TaskConsoleApp(taskService, taskPrinter);

app.Run();


