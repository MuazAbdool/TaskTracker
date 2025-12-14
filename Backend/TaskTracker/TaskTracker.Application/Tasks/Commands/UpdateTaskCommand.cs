using MediatR;

namespace TaskTracker.Application.Tasks.Commands;

public class UpdateTaskCommand : IRequest<int>
{
    public int Id { get; set; }
    public string Title { get; set; } 
    public string Description { get; set; }
    public Tastracker.Domain.Enums.Status Status { get; set; }
    public Tastracker.Domain.Enums.Priority Priority { get; set; }
    public DateTime? DueDate { get; set; }

   
    
}

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, int>
{
    private readonly Tastracker.Domain.Interfaces.Repositories.ITaskRepository _taskRepository;

    public UpdateTaskCommandHandler(Tastracker.Domain.Interfaces.Repositories.ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<int> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var existingTask = await _taskRepository.GetByIdAsync(request.Id);
        if (existingTask == null)
        {
            throw new KeyNotFoundException($"Task with ID {request.Id} was not found.");

        }

        existingTask.Title = request.Title;
        existingTask.Description = request.Description;
        existingTask.Status = request.Status;
        existingTask.Priority = request.Priority;
        existingTask.DueDate = request.DueDate;

        await  _taskRepository.UpdateAsync(existingTask);
        return request.Id;
    }
}