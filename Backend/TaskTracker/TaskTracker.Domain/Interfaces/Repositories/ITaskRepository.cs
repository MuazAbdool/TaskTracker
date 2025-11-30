using Task = Tastracker.Domain.Entities.Task;

namespace Tastracker.Domain.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<Task>GetByIdAsync(int id);
    Task<IEnumerable<Task>>GetAllAsync();
    Task<Task> AddAsync(Task task);
    Task<Task> UpdateAsync(Task task);
    Task<Task> DeleteAsync(int id);
}