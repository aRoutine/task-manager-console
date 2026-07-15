using TaskManager.Models;

namespace TaskManager.Interfaces;

public interface ITaskStorage
{
    List<TaskItem> LoadTasks();
    void SaveTasks(List<TaskItem> tasks);
}