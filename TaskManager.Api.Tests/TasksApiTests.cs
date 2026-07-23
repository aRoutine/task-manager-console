using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManager.Api.Contracts;
using TaskManager.Api.Tests;
using TaskManager.Models;

public class TasksApiTests
{
    [Fact]
    public async Task CreateTask_WithValidRequest_ShouldReturnCreated()
    {
        //Arrange
        await using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();

        HttpClient client = factory.CreateClient();

        CreateTaskRequest request = new CreateTaskRequest
        {
            Title = "valid title",
            Priority = TaskPriority.Medium
        };

        HttpResponseMessage response = await client.PostAsJsonAsync("/api/tasks", request);

        //Act
        List<TaskResponse>? tasks = await client.GetFromJsonAsync<List<TaskResponse>>("/api/tasks");


        //Assert
        Assert.NotNull(tasks);
        Assert.Single(tasks);
        Assert.Equal("valid title", tasks[0].Title);
        Assert.Equal(TaskPriority.Medium, tasks[0].TaskPriority);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateTask_WithEmptyTitle_ShouldReturnBadRequest()
    {
        await using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();

        HttpClient client = factory.CreateClient();

        CreateTaskRequest request = new CreateTaskRequest
        {
          Title = "",
          Priority = TaskPriority.Low  
        };

        HttpResponseMessage response = await client.PostAsJsonAsync("/api/tasks", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTasks_ShouldReturnOk()
    {
        await using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();

        HttpClient client = factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("/api/tasks");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}