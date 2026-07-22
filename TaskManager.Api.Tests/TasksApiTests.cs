using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManager.Api.Contracts;
using TaskManager.Models;

public class TasksApiTests
{
    [Fact]
    public async Task CreateTask_WithValidRequest_ShouldReturnCreated()
    {
        await using WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>();

        HttpClient client = factory.CreateClient();

        CreateTaskRequest request = new CreateTaskRequest
        {
            Title = "valid title",
            Priority = TaskPriority.Medium
        };

        HttpResponseMessage response = await client.PostAsJsonAsync("api/tasks", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreatedTask_WithEmptyTitle_ShouldReturnBadRequest()
    {
        await using WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>();

        HttpClient client = factory.CreateClient();

        CreateTaskRequest request = new CreateTaskRequest
        {
          Title = "",
          Priority = TaskPriority.Low  
        };

        HttpResponseMessage response = await client.PostAsJsonAsync("api/tasks", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTasks_ShouldRetunOk()
    {
        await using WebApplicationFactory<Program> factory = new WebApplicationFactory<Program>();

        HttpClient client = factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync("api/tasks");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}