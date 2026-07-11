namespace TaskManager.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsComplete { get; set; }

    public TaskItem(int id, string title)
    {
        Id = id;
        Title = title;
        IsComplete = false;
    }
}