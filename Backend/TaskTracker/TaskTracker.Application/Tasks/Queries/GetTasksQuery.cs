using MediatR;
using TaskTracker.Application.ModelExtensions;
using Tastracker.Domain.DTOS;
using Tastracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Tasks.Queries;

public class GetTasksQuery:IRequest<TaskListDto>
{
    public string ? SearchTerm { get; set; }
    public string  DueDateOrder { get; set; } 

    public GetTasksQuery( string ? searchTerm, string dueDateOrder)
    {
        SearchTerm = searchTerm;
        DueDateOrder = string.IsNullOrWhiteSpace(dueDateOrder)
            ? "dueDate:asc"
            : dueDateOrder;
    }   
    
}

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, TaskListDto>
{
    private readonly ITaskRepository _taskRepository;

    public GetTasksQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public  Task<TaskListDto> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var query = _taskRepository.Query(); 

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLowerInvariant();
            query = query.Where(t =>
                t.Title.Contains(term) ||
                (t.Description != null && t.Description.Contains(term))
            );
        }

        query = request.DueDateOrder.ToLower() switch
        {
            "duedate:desc" => query.OrderByDescending(t => t.DueDate),
            _ => query.OrderBy(t => t.DueDate)
        };
        // Using synchronous enumeration because the repository
       // returns an in-memory IQueryable (no IAsyncQueryProvider)

        var tasks = query.ToList();

        var taskListDto = new TaskListDto
        {
           Tasks = tasks.Select(s=>s.MapToDto()),
            TotalCount = tasks.Count
        };

        return Task.FromResult(taskListDto);
    }
}

