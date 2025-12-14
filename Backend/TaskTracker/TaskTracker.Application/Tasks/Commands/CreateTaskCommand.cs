using MediatR;
using Tastracker.Domain.DTOS;
using Tastracker.Domain.Entities;
using Tastracker.Domain.Interfaces.Repositories;
using TaskTracker.Application.ModelExtensions;
using Tastracker.Domain.Enums;
using Task = Tastracker.Domain.Entities.Task;

namespace TaskTracker.Application.Tasks.Commands
{
    // Command: encapsulates the data needed to create a task
    public class CreateTaskCommand : IRequest<TaskDto>
    {
        public string Title { get; set; } = string.Empty;
        public string?  Description { get; set; } 
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;

        public CreateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            // Map command to domain entity
            var task = new Task()
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                Priority = request.Priority,
                DueDate = request.DueDate,
            };

            await _taskRepository.AddAsync(task);

            // Map domain entity to DTO to return
            return task.MapToDto();
        }
    }
}