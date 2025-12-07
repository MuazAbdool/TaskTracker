using MediatR;
using Tastracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Tasks.Commands;

public class DeleteTaskCommand : IRequest<int>
{
    public int Id { get; set; }

    public DeleteTaskCommand( int id)
    {
        Id = id;   
    }
    
}

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, int>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<int> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var existingTask = await _taskRepository.GetByIdAsync(request.Id);
        if (existingTask == null)
        {
            throw new KeyNotFoundException($"Task with ID {request.Id} was not found.");

        }

        await  _taskRepository.DeleteAsync(request.Id);
        return request.Id;
    }
}