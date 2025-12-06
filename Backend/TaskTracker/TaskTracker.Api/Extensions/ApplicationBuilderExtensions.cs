
using TaskTracker.Api.Middleware;

namespace TaskTracker.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void Configure(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsProduction())
        {
            app.UseHttpsRedirection();
        }
        else
        {
            app.UseSwaggerMiddleware();
        }

        app.UseRouting();

        app.UseCors(ServiceCollectionExtensions.MyCorsPolicy);
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();
        

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        
    }
}