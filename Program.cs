using TaskManager.Services;
using TaskManager.UI;

TaskService taskService = new TaskService();
ConsoleTaskPrinter taskPrinter = new ConsoleTaskPrinter();

TaskConsoleApp App = new TaskConsoleApp(taskService, taskPrinter);

App.Run();


