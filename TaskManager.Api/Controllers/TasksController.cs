using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Contracts;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Results;

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

    private static TaskOperationResponse MapToOperationResponse(TaskOperationResult result)
    {
        return new TaskOperationResponse
        {
            Message = result.Message,
            Success = result.Success,
            TaskId = result.TaskId
        };
    }

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskResponse>>> GetTasks([FromQuery] TaskFilterRequest request)
    {
        List<TaskItem> tasks = await _taskService.GetTasksAsync(
            isComplete: request.IsComplete,
            priority: request.Priority,
            search: request.Search
        );

        return Ok(MapToResponseList(tasks));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponse>> GetTaskById(int id)
    {
        TaskItem? task = await _taskService.GetTaskByIdAsync(id);

        if (task == null)
        {
            return NotFound(new TaskOperationResponse
            {
                Success = false,
                Message = "Задача по заданному id не найдена"
            });
        }

        return Ok(MapToResponse(task));
    }

    [HttpGet("completed")]
    public async Task<ActionResult<List<TaskResponse>>> GetCompletedTasks()
    {
        List<TaskItem> tasks = await _taskService.GetCompletedTasksAsync();

        return Ok(MapToResponseList(tasks));
    }

    [HttpGet("not-completed")]
    public async Task<ActionResult<List<TaskResponse>>> GetNotCompletedTasks()
    {
        List<TaskItem> tasks = await _taskService.GetNotCompletedTasksAsync();

        return Ok(MapToResponseList(tasks));
    }

    [HttpGet("high-priority")]
    public async Task<ActionResult<List<TaskResponse>>> GetHighPriorityTasks()
    {
        List<TaskItem> tasks = await _taskService.GetHighPriorityTasksAsync();

        return Ok(MapToResponseList(tasks));
    }

    [HttpPost]
    public async Task<ActionResult> CreateTask(CreateTaskRequest request)
    {
        TaskOperationResult result = await _taskService.AddTaskAsync(request.Title, request.Priority);

        TaskOperationResponse response = MapToOperationResponse(result);

        if (!result.Success)
        {
            return BadRequest(response);
        }

        return CreatedAtAction(
            nameof(GetTaskById),
            new {id = response.TaskId},
            response
        );
    }

    [HttpPut("{id:int}/complete")]
    public async Task<ActionResult> CompleteTask(int id)
    {
        TaskOperationResult result = await _taskService.CompleteTaskAsync(id);

        TaskOperationResponse response = MapToOperationResponse(result);

        if (!result.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPut("{id:int}/rename")]
    public async Task<ActionResult> RenameTask(int id, RenameTaskRequest request)
    {
        TaskOperationResult result = await _taskService.RenameTaskAsync(id, request.Title);

        TaskOperationResponse response = MapToOperationResponse(result);

        if (!result.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        TaskOperationResult result = await _taskService.DeleteTaskAsync(id);

        TaskOperationResponse response = MapToOperationResponse(result);

        if (!result.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }
}