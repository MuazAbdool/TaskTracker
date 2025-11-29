using Microsoft.EntityFrameworkCore;
using Task = Tastracker.Domain.Entities.Task;

namespace TaskTracker.Infrustructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

 public DbSet<Task> Tasks => Set<Task>();

 public override int SaveChanges()
 {
     foreach (var entry in ChangeTracker.Entries<Task>())
     {
         if(entry.State == EntityState.Added)
         {
             entry.Entity.CreatedAt = DateTime.UtcNow;
         }
     }
        return base.SaveChanges();
 }
 
}