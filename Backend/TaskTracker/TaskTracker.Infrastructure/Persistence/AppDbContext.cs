using Microsoft.EntityFrameworkCore;
using TaskItem = Tastracker.Domain.Entities.Task;

namespace TaskTracker.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

 public DbSet<TaskItem> Tasks => Set<TaskItem>();

 public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
 {
     foreach (var entry in ChangeTracker.Entries<TaskItem>())
     {
         if (entry.State == EntityState.Added)
         {
             entry.Entity.CreatedAt = DateTime.UtcNow;
         }
     }

     return await base.SaveChangesAsync(cancellationToken);
 }
 
}