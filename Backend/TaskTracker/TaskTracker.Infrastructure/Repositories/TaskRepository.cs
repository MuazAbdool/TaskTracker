using Microsoft.EntityFrameworkCore;
using TaskTracker.Infrastructure.Persistence;
using Tastracker.Domain.Interfaces.Repositories;
using Task = Tastracker.Domain.Entities.Task;

namespace TaskTracker.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{   
    private readonly AppDbContext _db;
    public TaskRepository(AppDbContext db)
    {
        _db = db;
    }
    public Task<Task> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Task>> GetAllAsync()
    {
       return await _db.Tasks.ToListAsync();
    }

    public Task AddAsync(Task task)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Task task)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}