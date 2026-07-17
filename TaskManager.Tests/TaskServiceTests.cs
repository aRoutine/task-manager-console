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
        TaskService fakeService = new TaskService(fakeStorage);

        // Act
        var result = fakeService.AddTask("Изучение английского языка", Models.TaskPriority.Medium);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Задача успешно добавлена", result.Message);
        Assert.Equal(1, fakeStorage.SaveCallCount);
    }
}