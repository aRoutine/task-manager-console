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

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public ActionResult<List<TaskItem>> GetTasks()
    {
        return Ok(_taskService.GetTasks());
    }

    [HttpGet("completed")]
    public ActionResult<List<TaskItem>> GetCompletedTasks()
    {
        return Ok(_taskService.GetCompletedTasks());
    }

    [HttpGet("not-completed")]
    public ActionResult<List<TaskItem>> GetNotCompletedTasks()
    {
        return Ok(_taskService.GetNotCompletedTasks());
    }

    [HttpGet("high-priority")]
    public ActionResult<List<TaskItem>> GetHighPriorityTasks()
    {
        return Ok(_taskService.GetHighPriorityTasks());
    }

    [HttpPost]
    public ActionResult CreateTask(CreateTaskRequest request)
    {
        TaskOperationResult result = _taskService.AddTask(request.Title, request.Priority);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Created("api/tasks", result.Message);
    }

    [HttpPut("{id:int}/complete")]
    public ActionResult CompleteTask(int id)
    {
        TaskOperationResult result = _taskService.CompleteTask(id);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Message);
    }

    [HttpPut("{id:int}/rename")]
    public ActionResult RenameTask(int id, RenameTaskRequest request)
    {
        TaskOperationResult result = _taskService.RenameTask(id, request.Title);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Message);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteTask(int id)
    {
        TaskOperationResult result = _taskService.DeleteTask(id);

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Message);
    }
}