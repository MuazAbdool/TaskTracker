using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace TaskTracker.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            var problem = new ProblemDetails
            {
                Type = "https://example.com/probs/internal-server-error",
                Title = "An unexpected error occurred",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = ex.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = problem.Status.Value;
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}