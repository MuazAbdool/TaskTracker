using MediatR;
using TaskTracker.Application.ModelExtensions;
using Tastracker.Domain.DTOS;
using Tastracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Tasks.Queries;

public class GetTaskByIdQuery : IRequest<TaskDto>
{
    public int Id { get; }

    public GetTaskByIdQuery(int id)
    {
        Id = id;
    }
}

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id);

        if (task == null)
        {
            return null!;
        }

        return task.MapToDto();
    }
}