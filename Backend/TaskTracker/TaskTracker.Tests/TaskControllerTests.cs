using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Controllers;
using TaskItem = Tastracker.Domain.Entities.Task;
using Tastracker.Domain.Enums;
using Tastracker.Domain.Interfaces.Repositories;
using MediatR;
using TaskTracker.Application.Tasks.Commands;
using Tastracker.Domain.DTOS;

namespace TaskTracker.Tests
{
    [TestFixture]
    public class TasksControllerTests
    {
        private Mock<ITaskRepository> _taskRepoMock = null!;
        private TasksController _controller = null!;
        private Mock<IMediator> _mediatorMock = null!;

        [SetUp]
        public void Setup()
        {
            _taskRepoMock = new Mock<ITaskRepository>();
            _mediatorMock = new Mock<IMediator>();
            _controller = new TasksController(_taskRepoMock.Object, _mediatorMock.Object);
          
        }

        #region GetAll Tests
        [Test]
        public async Task GetAll_WhenCalled_ReturnsTasksWithTotalCount()
        {
            // Arrange
            var seededTasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Id = 1,
                    Title = "Initial Task",
                    Description = "Seeded task",
                    Status = Status.New,
                    Priority = Priority.Low,
                    DueDate = DateTime.UtcNow.AddDays(2),
                    CreatedAt = DateTime.UtcNow
                }
            };

            _taskRepoMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(seededTasks);

            // Act
            var actionResult = await _controller.GetAll("");

            // Assert
            var ok = actionResult as OkObjectResult;
            Assert.NotNull(ok);

            // Assert
            var dto = ok!.Value as TaskListDto;
            Assert.NotNull(dto);

            // Assert: Task list and count
            Assert.AreEqual(1, dto!.TotalCount);
            Assert.AreEqual(1, dto.Tasks.Count());
            Assert.AreEqual("Initial Task", dto.Tasks.First().Title);
        }
        #endregion

        #region GetById Tests
        [Test]
        public async Task GetById_TaskExists_ReturnsOk()
        {
            // Arrange
            var task = new TaskItem { Id = 1, Title = "Test Task" };
            _taskRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var returnedTask = result!.Value as TaskItem;
            Assert.NotNull(returnedTask);
            Assert.AreEqual(1, returnedTask!.Id);
        }

        [Test]
        public async Task GetById_TaskNotFound_ReturnsProblemDetails()
        {
            // Arrange
            _taskRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskItem?)null);

            // Act
            var result = await _controller.GetById(123) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            var problem = result!.Value as ProblemDetails;
            Assert.NotNull(problem);
            Assert.AreEqual(404, problem!.Status);
            Assert.AreEqual("Task not found", problem.Title);
        }
        #endregion

        #region Create Tests
        [Test]
        public async Task Create_ValidTask_ReturnsCreatedAtAction()
        {
            // Arrange
            var taskDto = new TaskDto { Id = 1, Title = "New Task", Status = Status.New, Priority = Priority.Low };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateTaskCommand>(), default))
                .ReturnsAsync(taskDto);

            // Act
            var result = await _controller.Create(new CreateTaskCommand()) as CreatedAtActionResult;
            var returnedDto = result.Value as TaskDto;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(nameof(_controller.GetById), result!.ActionName);
            Assert.AreEqual(taskDto.Id, returnedDto!.Id);
        }

        [Test]
        public async Task Create_InvalidTask_ReturnsValidationProblem()
        {
            // Arrange
            var task = new TaskItem { Id = 2, Title = "" }; // invalid
            _controller.ModelState.AddModelError("Title", "The Title field is required.");

            // Act
            var result = await _controller.Create(new CreateTaskCommand())  as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var problem = result!.Value as ValidationProblemDetails;
            Assert.NotNull(problem);
            Assert.IsTrue(problem.Errors.ContainsKey("Title"));
            Assert.AreEqual("The Title field is required.", problem.Errors["Title"][0]);
        }
        #endregion

        #region Update Tests
        [Test]
        public async Task Update_IdMismatch_ReturnsBadRequestProblemDetails()
        {
            // Arrange
            var task = new TaskItem { Id = 1, Title = "Task" };

            // Act
            var result = await _controller.Update(2, task) as BadRequestObjectResult;

            // Assert
            var problem = result!.Value as ProblemDetails;
            Assert.NotNull(problem);
            Assert.AreEqual(400, problem!.Status);
        }

        [Test]
        public async Task Update_ValidTask_ReturnsNoContent()
        {
            // Arrange
            var task = new TaskItem { Id = 1, Title = "Task" };
            _taskRepoMock.Setup(r => r.UpdateAsync(task)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, task) as NoContentResult;

            // Assert
            Assert.NotNull(result);
        }
        #endregion

        #region Delete Tests
        [Test]
        public async Task Delete_TaskExists_ReturnsNoContent()
        {
            // Arrange
            int taskId = 1;
            _mediatorMock
                .Setup(m => m.Send(It.Is<DeleteTaskCommand>(c => c.Id == taskId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(taskId);

            // Act
            var result = await _controller.Delete(taskId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Delete_TaskNotFound_ReturnsProblemDetails()
        {
            // Arrange
            int taskId = 123;
            _mediatorMock
                .Setup(m => m.Send(It.Is<DeleteTaskCommand>(c => c.Id == taskId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException($"Task with ID {taskId} not found."));

            // Act
            var result = await _controller.Delete(taskId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);

            var problem = notFoundResult!.Value as ProblemDetails;
            Assert.NotNull(problem);

            Assert.AreEqual(404, problem!.Status);
            Assert.AreEqual("Task not found", problem.Title);
        }

        #endregion
    }
}
