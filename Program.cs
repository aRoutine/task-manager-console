using TaskManager.Services;
using TaskManager.UI;

TaskService taskService = new TaskService();
ConsoleTaskPrinter taskPrinter = new ConsoleTaskPrinter();

TaskConsoleApp app = new TaskConsoleApp(taskService, taskPrinter);

app.Run();


