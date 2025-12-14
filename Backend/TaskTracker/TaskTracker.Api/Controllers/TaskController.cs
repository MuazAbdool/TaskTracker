using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Tasks.Commands;
using TaskTracker.Application.Tasks.Queries;
using Tastracker.Domain.DTOS;
using Tastracker.Domain.Interfaces.Repositories;
using Task = Tastracker.Domain.Entities.Task;

namespace TaskTracker.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{  
    private readonly IMediator _mediator;
    public TasksController( IMediator mediator)
    {
       
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll( [FromQuery] string ? q ,  [FromQuery] string? sort = "dueDate:asc")
    {
        var request = new GetTasksQuery(q, sort ?? "dueDate:asc");
        var taskListDto = await _mediator.Send(request);
        return Ok(taskListDto);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _mediator.Send(new GetTaskByIdQuery(id));

        if (task == null)
        {
            return NotFound(new ProblemDetails
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
        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, new TaskDto(createdTask));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskCommand updateTaskCommand)
    { 
        if(id != updateTaskCommand.Id)
        {
            return BadRequest(new ProblemDetails()
            {
                Type = "https://example.com/probs/bad-request",
                Title = "Bad Request",
                Status = StatusCodes.Status400BadRequest,
                Detail = "ID in the URL does not match ID in the body."
            });
        }
        try
        {
            await _mediator.Send(updateTaskCommand);
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