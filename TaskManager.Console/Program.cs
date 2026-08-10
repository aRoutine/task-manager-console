using TaskManager.Interfaces;
using TaskManager.Services;
using TaskManager.Storage;
using TaskManager.UI;

ITaskRepository taskRepository = new JsonTaskRepository();
ICurrentUserService fakeCurrentUserService = new FakeCurrentUserService();
ITaskService taskService = new TaskService(taskRepository, fakeCurrentUserService);
ITaskPrinter taskPrinter = new ConsoleTaskPrinter();

TaskConsoleApp app = new TaskConsoleApp(taskService, taskPrinter);

app.Run();


