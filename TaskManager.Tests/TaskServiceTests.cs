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
        FakeTaskStorage fakeStorage = new FakeTaskStorage();
        TaskService service = new TaskService(fakeStorage);

        // Act
        var result = service.AddTask("valid name", TaskPriority.Medium);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Задача успешно добавлена", result.Message);
        Assert.Equal(1, fakeStorage.SaveCallCount);
    }

    [Fact]
    public void AddTask_WithEmptyTitle_ShouldReturnFail()
    {
        //Arrange
        FakeTaskStorage storage = new FakeTaskStorage();
        TaskService service = new TaskService(storage);

        //Act
        var result = service.AddTask("", TaskPriority.Medium);

        //Assert
        Assert.False(result.Success);
        Assert.Equal("Название задачи не может быть пустым", result.Message);
        Assert.Equal(0, storage.SaveCallCount);
    }

    [Fact]
    public void DeleteTask_WithUnknownId_ShouldReturnFalse()
    {
        //Arrange
        FakeTaskStorage storage = new FakeTaskStorage();
        TaskService service = new TaskService(storage);

        //Act
        var result = service.DeleteTask(3);

        //Assert
        Assert.False(result.Success);
        Assert.Equal("Задача по заданному Id не была найдена в базе данных", result.Message);
        Assert.Equal(0, storage.SaveCallCount);
    }

    [Fact]
    public void CompleteTask_WithExistingTask_ShouldReturnSuccess()
    {
        //Arrange
        FakeTaskStorage storage = new FakeTaskStorage();
        TaskService service = new TaskService(storage);
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
        FakeTaskStorage storage = new FakeTaskStorage();
        TaskService service = new TaskService(storage);
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
        FakeTaskStorage storage = new FakeTaskStorage();
        TaskService service = new TaskService(storage);

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
        FakeTaskStorage storage = new FakeTaskStorage();
        TaskService service = new TaskService(storage);

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