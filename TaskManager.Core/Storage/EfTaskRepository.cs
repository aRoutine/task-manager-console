using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Storage;

public class EfTaskRepository : ITaskRepository
{
    private readonly TaskManagerDbContext _dbContext;

    public EfTaskRepository(TaskManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddTask(TaskItem task)
    {
        _dbContext.Tasks.Add(task);
    }

    public void DeleteTask(TaskItem task)
    {
        _dbContext.Tasks.Remove(task);
    }

    public List<TaskItem> GetAll()
    {
        return _dbContext.Tasks
            .AsNoTracking()
            .ToList();
    }

    public TaskItem? GetById(int id)
    {
        return _dbContext.Tasks.FirstOrDefault(t => t.Id == id);
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }

    public void Update(TaskItem task)
    {
        _dbContext.Tasks.Update(task);
    }
}