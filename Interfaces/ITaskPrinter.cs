using TaskManager.Models;

namespace TaskManager.Interfaces;

public interface ITaskPrinter
{
    void PrintTasks(List<TaskItem> tasks);
}