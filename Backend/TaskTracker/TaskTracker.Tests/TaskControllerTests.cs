using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Controllers;
using TaskItem = Tastracker.Domain.Entities.Task;
using Tastracker.Domain.Enums;
using Tastracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Tests
{
    public class TaskControllerTests
    {
        private Mock<ITaskRepository> _taskRepoMock = null!;
        private TasksController _controller = null!;

        [NUnit.Framework.SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _controller = new TasksController(_taskRepoMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsSeededData_HappyPath()
        {
            // Arrange
            var seededTasks = new List<Task>
            {
                new Task
                {
                    Id = 1,
                    Title = "Initial Task 1",
                    Description = "Seeded task",
                    Status = Status.New,
                    Priority = Priority.Low,
                    DueDate = DateTime.UtcNow.AddDays(2),
                    CreatedAt = DateTime.UtcNow
                }
            };
            _taskRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(seededTasks);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var tasks = result!.Value as IEnumerable<Task>;
            Assert.NotNull(tasks);
            Assert.AreEqual(1, tasks!.Count());
            Assert.AreEqual("Initial Task 1", tasks.First().Title);
        }

        [Test]
        public async Task Create_InvalidTask_ReturnsBadRequestWithProblemDetails()
        {
            // Arrange: create a task with missing Title
            var invalidTask = new Task
            {
                Id = 2,
                Title = "", // invalid
                Description = "No title",
                Status = Status.New,
                Priority = Priority.Low,
                DueDate = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow
            };

            _controller.ModelState.AddModelError("Title", "The Title field is required.");

            // Act
            var result = await _controller.Create(invalidTask) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var problem = result!.Value as ValidationProblemDetails;
            Assert.NotNull(problem);
            Assert.That(problem!.Errors.ContainsKey("Title"));
            Assert.That(problem.Errors["Title"][0], Is.EqualTo("The Title field is required."));
        }
    }
}
