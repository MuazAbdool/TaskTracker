using TaskTracker.Infrastructure.Persistence;
using Tastracker.Domain.Entities;
using Tastracker.Domain.Enums;
using Task = Tastracker.Domain.Entities.Task;

namespace TaskTracker.Infrastructure.Extensions;

public static class SeedDataExtensions
{
    public static void SeedData(this AppDbContext db)
    {
        if (db.Tasks.Any())
            return;

        db.Tasks.AddRange(
            new Task
            {
                Id = 1,
                Title = "Initial Task 1",
                Description = "Seeded task",
                Status = Status.New,
                Priority = Priority.Low,
                DueDate = DateTime.UtcNow.AddDays(2),
                CreatedAt = DateTime.UtcNow
            },
            new Task
            {
                Id = 2,
                Title = "Initial Task 2",
                Description = "Another seeded entry",
                Status = Status.InProgress,
                Priority = Priority.Medium,
                DueDate =  DateTime.UtcNow.AddDays(2),
                CreatedAt = DateTime.UtcNow
            }, 
            new Task
            {
                Id = 3,
                Title = "Initial Task 3",
                Description = "Another seeded entry",
                Status = Status.Done,
                Priority = Priority.High,
                DueDate =  DateTime.UtcNow.AddDays(10),
                CreatedAt = DateTime.UtcNow
            }, 
            new Task
            {
                Id = 4,
                Title = "Initial Task 4",
                Description = "Another seeded entry",
                Status = Status.Done,
                Priority = Priority.High,
                DueDate =  DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            }
        );

        db.SaveChanges();
    }
}