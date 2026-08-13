using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Contracts;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Results;

namespace TaskManager.Api.Controllers;

[Authorize]
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
        return tasks
            .Select(MapToResponse)
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
    public async Task<ActionResult<PagedResponse<TaskResponse>>> GetTasks([FromQuery] TaskFilterRequest request)
    {
        TaskQueryParameters parameters = new TaskQueryParameters
        {
            IsComplete = request.IsComplete,
            Priority = request.Priority,
            Page = request.Page,
            PageSize = request.PageSize,
            Search = request.Search
        };

        PagedResult<TaskItem> result = await _taskService.GetPagedTasksAsync(parameters);

        PagedResponse<TaskResponse> response = new PagedResponse<TaskResponse>
        {
            Items = MapToResponseList(result.Items),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        };

        return Ok(response);
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

    [HttpPost]
    public async Task<ActionResult> CreateTask(CreateTaskRequest request)
    {
        TaskOperationResult result = await _taskService.AddTaskAsync(request.Title, request.Priority);

        TaskOperationResponse response = MapToOperationResponse(result);

        if (!result.Success)
        {
            return HandleTaskError(result);
        }

        return CreatedAtAction(
            nameof(GetTaskById),
            new { id = response.TaskId },
            response
        );
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateTask(int id, UpdateTaskRequest request)
    {
        TaskOperationResult result = await _taskService.UpdateTaskAsync(
            id,
            request.Title,
            request.Priority,
            request.IsComplete
            );

        TaskOperationResponse response = MapToOperationResponse(result);

        if (!result.Success)
        {
            return HandleTaskError(result);
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        TaskOperationResult result = await _taskService.DeleteTaskAsync(id);

        if (!result.Success)
        {
            return HandleTaskError(result);
        }

        return NoContent();
    }

    private ActionResult HandleTaskError(TaskOperationResult result)
    {
        TaskOperationResponse response = MapToOperationResponse(result);

        return result.Error switch
        {
            TaskOperationError.Validation => BadRequest(response),
            TaskOperationError.NotFound => NotFound(response),
            TaskOperationError.Conflict => Conflict(response),
            _ => BadRequest()
        };
    }
}