using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services;

public interface ITaskService
{
    Task<List<TaskResponseDto>> GetAllTasksAsync(int userId, TaskFilterDto filter);
    Task<TaskResponseDto?> GetTaskByIdAsync(int id, int userId);
    Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto, int userId);
    Task<TaskResponseDto?> UpdateTaskAsync(int id, UpdateTaskDto dto, int userId);
    Task<bool> DeleteTaskAsync(int id, int userId);
}

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskResponseDto>> GetAllTasksAsync(
        int userId, TaskFilterDto filter)
    {
        var query = _context.Tasks
            .Where(t => t.UserId == userId);

        if (filter.Status.HasValue)
            query = query.Where(t => t.Status == filter.Status.Value);

        if (filter.Priority.HasValue)
            query = query.Where(t => t.Priority == filter.Priority.Value);

        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return tasks.Select(MapToDto).ToList();
    }

    public async Task<TaskResponseDto?> GetTaskByIdAsync(int id, int userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        return task == null ? null : MapToDto(task);
    }

    public async Task<TaskResponseDto> CreateTaskAsync(
        CreateTaskDto dto, int userId)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            UserId = userId,
            Status = Models.TaskStatus.Todo,
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return MapToDto(task);
    }

    public async Task<TaskResponseDto?> UpdateTaskAsync(
        int id, UpdateTaskDto dto, int userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task == null) return null;

        if (dto.Title != null) task.Title = dto.Title;
        if (dto.Description != null) task.Description = dto.Description;
        if (dto.Status.HasValue) task.Status = dto.Status.Value;
        if (dto.Priority.HasValue) task.Priority = dto.Priority.Value;
        if (dto.DueDate.HasValue) task.DueDate = dto.DueDate.Value;

        task.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return MapToDto(task);
    }

    public async Task<bool> DeleteTaskAsync(int id, int userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task == null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }

    private static TaskResponseDto MapToDto(TaskItem task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        Status = task.Status.ToString(),
        Priority = task.Priority.ToString(),
        DueDate = task.DueDate,
        CreatedAt = task.CreatedAt,
        UpdatedAt = task.UpdatedAt,
    };
}