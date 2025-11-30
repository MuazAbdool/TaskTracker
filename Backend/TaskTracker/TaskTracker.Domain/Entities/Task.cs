using System.ComponentModel.DataAnnotations;
using Tastracker.Domain.Enums;

namespace Tastracker.Domain.Entities;

public class Task
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Title is required")]
    [MinLength(1, ErrorMessage = "Title cannot be empty")]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Required(ErrorMessage = "Status is required")]
    [EnumDataType(typeof(Status), ErrorMessage = "Status must be New, InProgress, or Done")]
    public Status Status { get; set;  }
    [Required(ErrorMessage = "Priority is required")]
    [EnumDataType(typeof(Priority), ErrorMessage = "Priority must be Low, Medium, or High")]
    public Priority Priority { get; set;  }
    public DateTime? DueDate { get; set;  }
    public DateTime CreatedAt { get; set;  }
    
}