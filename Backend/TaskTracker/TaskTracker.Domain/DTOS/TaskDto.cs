using Tastracker.Domain.Enums;

namespace Tastracker.Domain.DTOS;

public class TaskDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Status Status { get; set;  }
    public Priority Priority { get; set;  }
    public DateTime? DueDate { get; set;  }
    public DateTime CreatedAt { get; set;  }

    public TaskDto()
    {
        
    }
    public TaskDto( TaskDto dto)
    {
        Id = dto.Id;
        Title = dto.Title;
        Description = dto.Description;
        Status = dto.Status;
        Priority = dto.Priority;
        DueDate = dto.DueDate;
        CreatedAt = dto.CreatedAt;
    }
}