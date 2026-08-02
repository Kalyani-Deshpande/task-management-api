using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    /// <summary>Get all tasks for the authenticated user</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<TaskResponseDto>), 200)]
    public async Task<IActionResult> GetAll([FromQuery] TaskFilterDto filter)
    {
        var tasks = await _taskService.GetAllTasksAsync(GetUserId(), filter);
        return Ok(new { data = tasks, count = tasks.Count });
    }

    /// <summary>Get a task by ID</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskResponseDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id, GetUserId());
        if (task == null) return NotFound(new { message = "Task not found" });
        return Ok(new { data = task });
    }

    /// <summary>Create a new task</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponseDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new { message = "Title is required" });

        var task = await _taskService.CreateTaskAsync(dto, GetUserId());
        return CreatedAtAction(nameof(GetById), new { id = task.Id },
            new { data = task });
    }

    /// <summary>Update a task</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TaskResponseDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        var task = await _taskService.UpdateTaskAsync(id, dto, GetUserId());
        if (task == null) return NotFound(new { message = "Task not found" });
        return Ok(new { data = task });
    }

    /// <summary>Delete a task</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _taskService.DeleteTaskAsync(id, GetUserId());
        if (!deleted) return NotFound(new { message = "Task not found" });
        return Ok(new { message = "Task deleted successfully" });
    }

    /// <summary>Update task status only</summary>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(TaskResponseDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTaskDto dto)
    {
        var task = await _taskService.UpdateTaskAsync(id, dto, GetUserId());
        if (task == null) return NotFound(new { message = "Task not found" });
        return Ok(new { data = task });
    }
}