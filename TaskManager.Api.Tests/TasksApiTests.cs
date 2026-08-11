using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Contracts;
using TaskManager.Api.Tests;
using TaskManager.Models;
using TaskManager.Results;
using System.Net.Http.Headers;

public class TasksApiTests
{
    [Fact]
    public async Task CreateTask_WithValidRequest_ShouldReturnCreated()
    {
        //Arrange
        await using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();

        HttpClient client = factory.CreateClient();

        LoginRequest loginRequest = new LoginRequest
        {
            Email = "default_email@example.com",
            Password = "123456789a!_sl"
        };

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);

        AuthResponse? auth = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        CreateTaskRequest request = new CreateTaskRequest
        {
            Title = "valid title",
            Priority = TaskPriority.Medium
        };

        HttpResponseMessage response = await client.PostAsJsonAsync("/api/tasks", request);

        //Act
        PagedResponse<TaskResponse>? pagedResponse = await client.GetFromJsonAsync<PagedResponse<TaskResponse>>("/api/tasks");


        //Assert
        Assert.NotNull(pagedResponse);
        Assert.Single(pagedResponse.Items);
        Assert.Equal("valid title", pagedResponse.Items[0].Title);
        Assert.Equal(TaskPriority.Medium, pagedResponse.Items[0].TaskPriority);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateTask_WithEmptyTitle_ShouldReturnBadRequest()
    {
        await using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();

        HttpClient client = factory.CreateClient();

        LoginRequest loginRequest = new LoginRequest
        {
            Email = "default_email@example.com",
            Password = "123456789a!_sl"
        };

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);

        AuthResponse? auth = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

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

         LoginRequest loginRequest = new LoginRequest
        {
              Email = "default_email@example.com",
              Password = "123456789a!_sl"
        };

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);

        AuthResponse? auth= await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        HttpResponseMessage response = await client.GetAsync("/api/tasks");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTaskById_WithExistingId_ShouldReturnOk()
    {
        await using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();

        HttpClient client = factory.CreateClient();

         LoginRequest loginRequest = new LoginRequest
        {
              Email = "default_email@example.com",
              Password = "123456789a!_sl"
        };

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);

        AuthResponse? auth= await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        CreateTaskRequest request = new CreateTaskRequest
        {
            Title = "correct title",
            Priority = TaskPriority.Medium
        };

        await client.PostAsJsonAsync("/api/tasks", request);

        HttpResponseMessage response = await client.GetAsync("/api/tasks/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetTaskById_WithUnknownId_ShouldReturnNotFound()
    {
        await using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();

        HttpClient client = factory.CreateClient();

         LoginRequest loginRequest = new LoginRequest
        {
              Email = "default_email@example.com",
              Password = "123456789a!_sl"
        };

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);

        AuthResponse? auth= await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        HttpResponseMessage response = await client.GetAsync("/api/tasks/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetTasks_WithIsCompleteFalse_ShouldReturnOnlyNotCompletedTasks()
    {
        await using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();

        HttpClient client = factory.CreateClient();

         LoginRequest loginRequest = new LoginRequest
        {
              Email = "default_email@example.com",
              Password = "123456789a!_sl"
        };

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);

        AuthResponse? auth= await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        CreateTaskRequest request1 = new CreateTaskRequest
        {
            Title = "correct title",
            Priority = TaskPriority.High
        };

        await client.PostAsJsonAsync("api/tasks", request1);

        CreateTaskRequest request2 = new CreateTaskRequest
        {
            Title = "target",
            Priority = TaskPriority.High
        };

        await client.PostAsJsonAsync("api/tasks", request2);

        await client.PutAsync("api/tasks/1/complete", null);

        PagedResponse<TaskResponse>? response = await client.GetFromJsonAsync<PagedResponse<TaskResponse>>("/api/tasks?isComplete=false");

        Assert.NotNull(response);
        Assert.Single(response.Items);
        Assert.False(response.Items[0].IsComplete);
        Assert.Equal("target", response.Items[0].Title);
    }

    [Fact]
    public async Task GetTasks_WithPagination_ShouldReturnRequestedPage()
    {
        // Arrange
        await using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();

        HttpClient client = factory.CreateClient();

         LoginRequest loginRequest = new LoginRequest
        {
              Email = "default_email@example.com",
              Password = "123456789a!_sl"
        };

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);

        AuthResponse? auth= await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        await client.PostAsJsonAsync(
            "/api/tasks",
            new CreateTaskRequest
            {
                Title = "title 1",
                Priority = TaskPriority.Low
            }
        );

        await client.PostAsJsonAsync(
            "/api/tasks",
            new CreateTaskRequest
            {
                Title = "title 2",
                Priority = TaskPriority.Low
            }
        );

        await client.PostAsJsonAsync(
            "/api/tasks",
            new CreateTaskRequest
            {
                Title = "title 3",
                Priority = TaskPriority.Low
            }
        );

        // Act

        PagedResponse<TaskResponse>? response =
            await client.GetFromJsonAsync<PagedResponse<TaskResponse>>("/api/tasks?page=2&pageSize=1");

        // Assert
        Assert.NotNull(response);
        Assert.Single(response.Items);
        Assert.Equal("title 2", response.Items[0].Title);
        Assert.Equal(2, response.Page);
        Assert.Equal(1, response.PageSize);
        Assert.Equal(3, response.TotalCount);
        Assert.Equal(3, response.TotalPages);
    }

    [Fact]
    public async Task UpdateTask_WithValidRequest_ShouldReturnOk()
    {
        await using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();

        HttpClient client = factory.CreateClient();

         LoginRequest loginRequest = new LoginRequest
        {
              Email = "default_email@example.com",
              Password = "123456789a!_sl"
        };

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);

        AuthResponse? auth= await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        CreateTaskRequest createRequest = new CreateTaskRequest
        {
            Title = "valid title",
            Priority = TaskPriority.Low
        };

        await client.PostAsJsonAsync("/api/tasks", createRequest);

        UpdateTaskRequest request = new UpdateTaskRequest
        {
            Title = "new valid title",
            Priority = TaskPriority.High,
            IsComplete = true
        };

        HttpResponseMessage response = await client.PutAsJsonAsync("/api/tasks/1", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        TaskResponse? task = await client.GetFromJsonAsync<TaskResponse>("/api/tasks/1");

        Assert.NotNull(task);
        Assert.Equal("new valid title", task.Title);
        Assert.Equal(TaskPriority.High, task.TaskPriority);
        Assert.True(task.IsComplete);
    }

    [Fact]
    public async Task UpdateTask_WithUnknownId_ShouldReturnNotFound()
    {
        await using CustomWebApplicationFactory factory = new CustomWebApplicationFactory();

        HttpClient client = factory.CreateClient();

         LoginRequest loginRequest = new LoginRequest
        {
              Email = "default_email@example.com",
              Password = "123456789a!_sl"
        };

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);

        AuthResponse? auth= await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        CreateTaskRequest createRequest = new CreateTaskRequest
        {
            Title = "valid title",
            Priority = TaskPriority.Low
        };

        await client.PostAsJsonAsync("/api/tasks", createRequest);

        UpdateTaskRequest request = new UpdateTaskRequest
        {
            Title = "new valid title",
            Priority = TaskPriority.High,
            IsComplete = true
        };

        HttpResponseMessage response = await client.PutAsJsonAsync("/api/tasks/2", request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        TaskResponse? task = await client.GetFromJsonAsync<TaskResponse>("/api/tasks/1");

        Assert.NotNull(task);
        Assert.Equal("valid title", task.Title);
        Assert.Equal(TaskPriority.Low, task.TaskPriority);
        Assert.False(task.IsComplete);
    }

    [Fact]
    public async Task UnknownRoute_ShouldReturnNotFoundProblemDetails()
    {
        await using CustomWebApplicationFactory factory =
            new CustomWebApplicationFactory();

        HttpClient client = factory.CreateClient();

         LoginRequest loginRequest = new LoginRequest
        {
              Email = "default_email@example.com",
              Password = "123456789a!_sl"
        };

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", loginRequest);

        AuthResponse? auth= await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);

        HttpResponseMessage response =
            await client.GetAsync("/api/unknown-route");

        ProblemDetails? problemDetails =
            await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(problemDetails);
        Assert.Equal(404, problemDetails.Status);
    }

}