namespace Tastracker.Domain.DTOS;
using TaskItem = Tastracker.Domain.Entities.Task;
using System.Collections.Generic;
public class TaskListDto
{
    public IEnumerable<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public int TotalCount { get; set; }
}