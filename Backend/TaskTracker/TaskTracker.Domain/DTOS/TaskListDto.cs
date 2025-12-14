namespace Tastracker.Domain.DTOS;
using TaskItem = Tastracker.Domain.Entities.Task;
using System.Collections.Generic;
public class TaskListDto
{
    public IEnumerable<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    public int TotalCount { get; set; }
}