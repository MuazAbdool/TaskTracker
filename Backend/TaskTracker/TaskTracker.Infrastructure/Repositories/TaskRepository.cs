using Microsoft.EntityFrameworkCore;
using TaskTracker.Infrastructure.Persistence;
using Tastracker.Domain.Interfaces.Repositories;
using TaskItem = Tastracker.Domain.Entities.Task;

namespace TaskTracker.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{   
    private readonly AppDbContext _db;
    public TaskRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<TaskItem> GetByIdAsync(int id)
    {
     return await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
       return await _db.Tasks.ToListAsync();
    }

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        await _db.Tasks.AddAsync(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem> UpdateAsync(TaskItem task)
    {
        _db.Tasks.Update(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem> DeleteAsync(int id)
    {
        var existing = await _db.Tasks.FindAsync(id);
        if (existing != null)
        {
            _db.Tasks.Remove(existing);
            await _db.SaveChangesAsync();
            return existing;
        }

        return null;
    }
}