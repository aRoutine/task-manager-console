using TaskManager.Models;

namespace TaskManager.Interfaces;

public interface ITaskRepository
{
    List<TaskItem> GetAll();
    TaskItem? GetById(int id);
    void AddTask(TaskItem task);
    void Update(TaskItem task);
    void DeleteTask(TaskItem task);
    void SaveChanges();
}