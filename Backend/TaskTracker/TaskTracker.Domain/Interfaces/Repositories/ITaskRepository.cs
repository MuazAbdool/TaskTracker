using Task = Tastracker.Domain.Entities.Task;

namespace Tastracker.Domain.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<Task>GetByIdAsync(int id);
    Task<IEnumerable<Task>>GetAllAsync();
    Task AddAsync(Task task);
    Task UpdateAsync(Task task);
    Task DeleteAsync(int id);
}