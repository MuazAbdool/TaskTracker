using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Infrustructure.Persistence;
using TaskTracker.Infrustructure.Repositories;
using Tastracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Infrustructure.Extensions;

public static class InfrastructureExtensions
{
    public static void AddInfrastructureSupport(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("TasksDb");
        });
        RegisterRepositories(services);
    }
    private static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITaskRepository, TaskRepository>();
    }
}
