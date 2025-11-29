using Tastracker.Domain.Interfaces.Repositories;
using Task = Tastracker.Domain.Entities.Task;

namespace TaskTracker.Infrustructure.Repositories;

public class TaskRepository : ITaskRepository
{
    public Task<Task> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Task>> GetAllAsync()
    {
        throw new NotImplementedException();
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