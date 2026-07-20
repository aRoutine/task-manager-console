using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Contracts;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Results;
using TaskManager.Services;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    private static TaskResponse MapToResponse(TaskItem task)
    {
        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            IsComplete = task.IsComplete,
            CreatedAt = task.CreatedAt,
            TaskPriority = task.TaskPriority
        };
    }

    private static List<TaskResponse> MapToResponseList(List<TaskItem> tasks)
    {
        return tasks.
            Select(MapToResponse)
            .ToList();
    }

    private static TaskOperationResponse MapToOperationResponse(TaskOperationResult response)
    {
        return new TaskOperationResponse
        {
            Message = response.Message,
            Success = response.Success
        };
    }

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public ActionResult<List<TaskResponse>> GetTasks()
    {
        return Ok(MapToResponseList(_taskService.GetTasks()));
    }

    [HttpGet("completed")]
    public ActionResult<List<TaskResponse>> GetCompletedTasks()
    {
        return Ok(MapToResponseList(_taskService.GetCompletedTasks()));
    }

    [HttpGet("not-completed")]
    public ActionResult<List<TaskResponse>> GetNotCompletedTasks()
    {
        return Ok(MapToResponseList(_taskService.GetNotCompletedTasks()));
    }

    [HttpGet("high-priority")]
    public ActionResult<List<TaskResponse>> GetHighPriorityTasks()
    {
        return Ok(MapToResponseList(_taskService.GetHighPriorityTasks()));
    }

    [HttpPost]
    public ActionResult CreateTask(CreateTaskRequest request)
    {
        TaskOperationResult result = _taskService.AddTask(request.Title, request.Priority);

        TaskOperationResponse response = MapToOperationResponse(result);

        if (!result.Success)
        {
            return BadRequest(response);
        }

        return Created("api/tasks", response);
    }

    [HttpPut("{id:int}/complete")]
    public ActionResult CompleteTask(int id)
    {
        TaskOperationResult result = _taskService.CompleteTask(id);

        TaskOperationResponse response = MapToOperationResponse(result);

        if (!result.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPut("{id:int}/rename")]
    public ActionResult RenameTask(int id, RenameTaskRequest request)
    {
        TaskOperationResult result = _taskService.RenameTask(id, request.Title);

        TaskOperationResponse response = MapToOperationResponse(result);

        if (!result.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteTask(int id)
    {
        TaskOperationResult result = _taskService.DeleteTask(id);

        TaskOperationResponse response = MapToOperationResponse(result);

        if (!result.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }
}