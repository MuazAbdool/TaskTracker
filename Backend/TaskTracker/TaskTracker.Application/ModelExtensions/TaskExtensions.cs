using Tastracker.Domain.DTOS;


namespace TaskTracker.Application.ModelExtensions;

public static class TaskExtensions
{
    public static TaskDto MapToDto(this Tastracker.Domain.Entities.Task task)
    {
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
        };
    }
}