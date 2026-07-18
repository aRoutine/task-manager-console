using TaskManager.Results;
using TaskManager.Models;

namespace TaskManager.Interfaces;

public interface ITaskService
{
    TaskOperationResult AddTask(string title, TaskPriority taskPriority);

    List<TaskItem> GetTasks();

    List<TaskItem> GetCompletedTasks();

    List<TaskItem> GetNotCompletedTasks();

    List<TaskItem> GetHighPriorityTasks();

    TaskOperationResult CompleteTask(int id);

    TaskOperationResult DeleteTask(int id);

    TaskOperationResult RenameTask(int id, string title);
}