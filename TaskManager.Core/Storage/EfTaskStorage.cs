using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Interfaces;
using TaskManager.Models;

public class EfTaskStorage : ITaskStorage
{
    private TaskManagerDbContext _dbContext;

    public EfTaskStorage(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<TaskItem> LoadTasks()
    {
        return _dbContext.Tasks
            .AsNoTracking()
            .ToList();
    }

    public void SaveTasks(List<TaskItem> tasks)
    {
        _dbContext.Tasks.RemoveRange(_dbContext.Tasks);
        _dbContext.Tasks.AddRange(tasks);
        _dbContext.SaveChanges();
    }
}