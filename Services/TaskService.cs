using System.Diagnostics;
using TaskManager.models;

namespace TaskManager.services;

public class TaskService
{
    private List<TaskItem> _tasks = new ();
    private int nextId = 1;

    public void AddTask(string title)
    {
        if (title == null)
        {
            Console.WriteLine("Название задачи не может быть пустым");
            return;
        }

        TaskItem newTaskItem = new TaskItem(nextId, title);
        _tasks.Add(newTaskItem);
        nextId++;

    Console.WriteLine("Задача успешно добавлена!");
    }

    public void ShowTasks()
    {
        if (_tasks.Count == 0)
        {
            Console.WriteLine("В данный момент у вас нет никаких задач");
            return;
        }

        foreach (TaskItem taskItem in _tasks)
        {
            Console.WriteLine($"Задача: {taskItem.Title}");

            string status = "Не выполнено";
            if (taskItem.IsComplete == true)
                status = "Выполнено";

            Console.WriteLine($"Статус: {status}");

            Console.WriteLine("");
        }

        Console.WriteLine("Всего заданий: " + _tasks.Count);
    }

    public void DeleteTask(int id)
    {
        foreach (TaskItem taskItem in _tasks)
        {
            if (id == taskItem.Id)
            {
                _tasks.Remove(taskItem);
                Console.WriteLine("Задача успешно удалена !");
                return;
            }
        }

        Console.WriteLine("Задача с заданным Id не была найдена в базе данных");
    }
}