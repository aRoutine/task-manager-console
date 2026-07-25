using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Tests.Fakes;


namespace TaskManager.Tests;

public class TaskServiceTests
{
    [Fact]
    public void AddTask_WithValidTitle_ShouldReturnSuccess()
    {
        // Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        TaskService service = new TaskService(taskRepository: fakeTaskRepository);

        // Act
        var result = service.AddTask("valid name", TaskPriority.Medium);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Задача успешно добавлена", result.Message);
        Assert.Equal(1, fakeTaskRepository.SaveCallCount);
    }

    [Fact]
    public void AddTask_WithEmptyTitle_ShouldReturnFail()
    {
        //Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        TaskService service = new TaskService(taskRepository: fakeTaskRepository);

        //Act
        var result = service.AddTask("", TaskPriority.Medium);

        //Assert
        Assert.False(result.Success);
        Assert.Equal("Название задачи не может быть пустым", result.Message);
        Assert.Equal(0, fakeTaskRepository.SaveCallCount);
    }

    [Fact]
    public void DeleteTask_WithUnknownId_ShouldReturnFalse()
    {
        //Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        TaskService service = new TaskService(taskRepository: fakeTaskRepository);

        //Act
        var result = service.DeleteTask(3);

        //Assert
        Assert.False(result.Success);
        Assert.Equal("Задача по заданному Id не была найдена в базе данных", result.Message);
        Assert.Equal(0, fakeTaskRepository.SaveCallCount);
    }

    [Fact]
    public void CompleteTask_WithExistingTask_ShouldReturnSuccess()
    {
        //Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        TaskService service = new TaskService(taskRepository: fakeTaskRepository);
        service.AddTask("valid name", TaskPriority.Low);

        //Act
        var result = service.CompleteTask(1);

        //Assert
        Assert.True(result.Success);
        Assert.Equal("Задача успешно выполнена", result.Message);
    }

    [Fact]
    public void CompleteTask_WithAlreadyCompletedTask_ShouldReturnFail()
    {
        //Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        TaskService service = new TaskService(taskRepository: fakeTaskRepository);
        service.AddTask("valid name", TaskPriority.Low);
        service.CompleteTask(1);

        //Act
        var result = service.CompleteTask(1);

        //Assert
        Assert.False(result.Success);
        Assert.Equal("Задача уже выполнена", result.Message);
    }

    [Fact]
    public void CompleteTask_WithUnknownId_ShouldReturnFail()
    {
        //Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        TaskService service = new TaskService(taskRepository: fakeTaskRepository);

        //Act
        var result = service.CompleteTask(3);

        //Assert
        Assert.False(result.Success);
        Assert.Equal("Задача по заданному ID не найдена", result.Message);
    }

    [Fact]
    public void GetTasks_ShouldReturnTasksSortedByPriority()
    {
        //Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        TaskService service = new TaskService(taskRepository: fakeTaskRepository);

        service.AddTask("Важное задание", TaskPriority.High);
        service.AddTask("Неважное задание", TaskPriority.Low);
        service.AddTask("Обычное задание", TaskPriority.Medium);

        //Act
        var result = service.GetTasks();

        //Assert
        Assert.Equal(TaskPriority.High, result[0].TaskPriority);
        Assert.Equal(TaskPriority.Medium, result[1].TaskPriority);
        Assert.Equal(TaskPriority.Low, result[2].TaskPriority);
    }

}