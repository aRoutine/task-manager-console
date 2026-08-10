using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Tests.Fakes;


namespace TaskManager.Tests;

public class TaskServiceTests
{
    [Fact]
    public async Task AddTask_WithValidTitle_ShouldReturnSuccess()
    {
        // Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        ICurrentUserService currentUserService = new FakeCurrentUserService();
        TaskService service = new TaskService(fakeTaskRepository, currentUserService);

        // Act
        var result = await service.AddTaskAsync("valid name", TaskPriority.Medium);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Задача успешно добавлена", result.Message);
        Assert.Equal(1, fakeTaskRepository.SaveCallCount);
    }

    [Fact]
    public async Task AddTask_WithEmptyTitle_ShouldReturnFail()
    {
        //Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        ICurrentUserService currentUserService = new FakeCurrentUserService();
        TaskService service = new TaskService(fakeTaskRepository, currentUserService);

        //Act
        var result = await service.AddTaskAsync("", TaskPriority.Medium);

        //Assert
        Assert.False(result.Success);
        Assert.Equal("Название задачи не может быть пустым", result.Message);
        Assert.Equal(0, fakeTaskRepository.SaveCallCount);
    }

    [Fact]
    public async Task DeleteTask_WithUnknownId_ShouldReturnFalse()
    {
        //Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        ICurrentUserService currentUserService = new FakeCurrentUserService();
        TaskService service = new TaskService(fakeTaskRepository, currentUserService);

        //Act
        var result = await service.DeleteTaskAsync(3);

        //Assert
        Assert.False(result.Success);
        Assert.Equal("Задача по заданному Id не была найдена в базе данных", result.Message);
        Assert.Equal(0, fakeTaskRepository.SaveCallCount);
    }

    [Fact]
    public async Task CompleteTask_WithExistingTask_ShouldReturnSuccess()
    {
        //Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        ICurrentUserService currentUserService = new FakeCurrentUserService();
        TaskService service = new TaskService(fakeTaskRepository, currentUserService);
        await service.AddTaskAsync("valid name", TaskPriority.Low);

        //Act
        var result = await service.CompleteTaskAsync(1);

        //Assert
        Assert.True(result.Success);
        Assert.Equal("Задача успешно выполнена", result.Message);
    }

    [Fact]
    public async Task CompleteTask_WithAlreadyCompletedTask_ShouldReturnFail()
    {
        //Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        ICurrentUserService currentUserService = new FakeCurrentUserService();
        TaskService service = new TaskService(fakeTaskRepository, currentUserService);
        await service.AddTaskAsync("valid name", TaskPriority.Low);
        await service.CompleteTaskAsync(1);

        //Act
        var result = await service.CompleteTaskAsync(1);

        //Assert
        Assert.False(result.Success);
        Assert.Equal("Задача уже выполнена", result.Message);
    }

    [Fact]
    public async Task CompleteTask_WithUnknownId_ShouldReturnFail()
    {
        //Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        ICurrentUserService currentUserService = new FakeCurrentUserService();
        TaskService service = new TaskService(fakeTaskRepository, currentUserService);

        //Act
        var result = await service.CompleteTaskAsync(3);

        //Assert
        Assert.False(result.Success);
        Assert.Equal("Задача по заданному ID не найдена", result.Message);
    }

    [Fact]
    public async Task GetTasks_ShouldReturnTasksSortedByPriority()
    {
        //Arrange
        FakeTaskRepository fakeTaskRepository = new FakeTaskRepository();
        ICurrentUserService currentUserService = new FakeCurrentUserService();
        TaskService service = new TaskService(fakeTaskRepository, currentUserService);

        await service.AddTaskAsync("Важное задание", TaskPriority.High);
        await service.AddTaskAsync("Неважное задание", TaskPriority.Low);
        await service.AddTaskAsync("Обычное задание", TaskPriority.Medium);

        //Act
        var result = await service.GetTasksAsync();

        //Assert
        Assert.Equal(TaskPriority.High, result[0].TaskPriority);
        Assert.Equal(TaskPriority.Medium, result[1].TaskPriority);
        Assert.Equal(TaskPriority.Low, result[2].TaskPriority);
    }

}