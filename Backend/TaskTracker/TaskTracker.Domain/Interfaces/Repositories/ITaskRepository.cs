using TaskItem = Tastracker.Domain.Entities.Task;

namespace Tastracker.Domain.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<TaskItem>GetByIdAsync(int id);
    Task<IEnumerable<TaskItem>>GetAllAsync();
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(int id);
}