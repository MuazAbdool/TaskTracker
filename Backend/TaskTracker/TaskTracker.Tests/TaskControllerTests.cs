using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Api.Controllers;
using Tastracker.Domain.DTOS;
using Tastracker.Domain.Enums;
using MediatR;
using TaskTracker.Application.Tasks.Commands;
using TaskTracker.Application.Tasks.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskTracker.Tests
{
    [TestFixture]
    public class TasksControllerTests
    {
        private Mock<IMediator> _mediatorMock = null!;
        private TasksController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new TasksController(_mediatorMock.Object);
        }

        #region GetAll Tests
        [Test]
        public async Task GetAll_WhenCalled_ReturnsTasksWithTotalCount()
        {
            // Arrange
            var seededTasks = new List<TaskDto>
            {
                new TaskDto { Id = 1, Title = "Initial Task", Status = Status.New, Priority = Priority.Low }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetTasksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TaskListDto
                {
                    Tasks = seededTasks,
                    TotalCount = seededTasks.Count
                });

            // Act
            var actionResult = await _controller.GetAll(null, null);

            // Assert
            var ok = actionResult as OkObjectResult;
            Assert.NotNull(ok);

            var dto = ok!.Value as TaskListDto;
            Assert.NotNull(dto);
            Assert.AreEqual(1, dto.TotalCount);
            Assert.AreEqual("Initial Task", dto.Tasks.First().Title);
        }
        #endregion

        #region GetById Tests
        [Test]
        public async Task GetById_TaskExists_ReturnsOk()
        {
            // Arrange
            var taskDto = new TaskDto { Id = 1, Title = "Test Task" };
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(taskDto);

            // Act
            var result = await _controller.GetById(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var returnedTask = result!.Value as TaskDto;
            Assert.NotNull(returnedTask);
            Assert.AreEqual(1, returnedTask!.Id);
        }

        [Test]
        public async Task GetById_TaskNotFound_ReturnsProblemDetails()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TaskDto?)null);

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
                .Setup(m => m.Send(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(taskDto);

            // Act
            var result = await _controller.Create(new CreateTaskCommand()) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(nameof(_controller.GetById), result!.ActionName);
            var returnedDto = result.Value as TaskDto;
            Assert.AreEqual(taskDto.Id, returnedDto!.Id);
        }

        [Test]
        public async Task Create_InvalidTask_ReturnsValidationProblem()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "The Title field is required.");

            // Act
            var result = await _controller.Create(new CreateTaskCommand()) as BadRequestObjectResult;

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
            var command = new UpdateTaskCommand { Id = 1, Title = "Task" };

            // Act
            var result = await _controller.Update(2, command) as BadRequestObjectResult;

            // Assert
            var problem = result!.Value as ProblemDetails;
            Assert.NotNull(problem);
            Assert.AreEqual(400, problem!.Status);
        }

        [Test]
        public async Task Update_ValidTask_ReturnsNoContent()
        {
            // Arrange
            var command = new UpdateTaskCommand { Id = 1, Title = "Task" };
            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(command.Id);

            // Act
            var result = await _controller.Update(1, command) as NoContentResult;

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
            var result = await _controller.Delete(taskId) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            var problem = result!.Value as ProblemDetails;
            Assert.NotNull(problem);
            Assert.AreEqual(404, problem!.Status);
            Assert.AreEqual("Task not found", problem.Title);
        }
        #endregion
    }
}
