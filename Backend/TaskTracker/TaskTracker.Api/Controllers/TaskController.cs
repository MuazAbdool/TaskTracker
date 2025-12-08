using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Tasks.Commands;
using Tastracker.Domain.DTOS;
using Tastracker.Domain.Interfaces.Repositories;
using Task = Tastracker.Domain.Entities.Task;

namespace TaskTracker.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{  
    private readonly ITaskRepository _taskRepository;
    private readonly IMediator _mediator;
    public TasksController(ITaskRepository taskRepository , IMediator mediator)
    {
        _taskRepository = taskRepository;
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll( [FromQuery] string? q,[FromQuery] string sort = "dueDate:asc" )
    {
        var tasks = await _taskRepository.GetAllAsync();
        if (!string.IsNullOrWhiteSpace(q))
        {
            tasks = tasks
                .Where(t => t.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                            (t.Description != null && t.Description.Contains(q, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            
            
        }
        
        tasks = sort.ToLower() switch
        {
            "duedate:desc" => tasks.OrderByDescending(t => t.DueDate).ToList(),
            _ => tasks.OrderBy(t => t.DueDate).ToList()
        };
        var taskListDto = new TaskListDto
        {
            Tasks = tasks,
            TotalCount = tasks.Count()
        };
        return Ok(taskListDto);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound( new ProblemDetails()
            {
                Type = "https://example.com/probs/task-not-found",
                Title = "Task not found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"Task with ID {id} was not found."
            });
        }
        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ValidationProblemDetails(ModelState));

        var createdTask = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Task task)
    {
        if (id != task.Id)
            return BadRequest(new ProblemDetails()
            {
                Type = "https://example.com/probs/bad-request",
                Title = "Bad Request",
                Status = StatusCodes.Status400BadRequest,
                Detail = "ID in the URL does not match ID in the body."
            });
        
        await  _taskRepository.UpdateAsync(task);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deletedId = await _mediator.Send(new DeleteTaskCommand(id));
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://example.com/probs/task-not-found",
                Title = "Task not found",
                Status = StatusCodes.Status404NotFound,
                Detail = ex.Message
            });
        }
    }
}