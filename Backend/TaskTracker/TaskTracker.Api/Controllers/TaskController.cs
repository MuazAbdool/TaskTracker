using Microsoft.AspNetCore.Mvc;
using Tastracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{  
    private readonly ITaskRepository _taskRepository;
    public TaskController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return Ok(tasks);
    }
}