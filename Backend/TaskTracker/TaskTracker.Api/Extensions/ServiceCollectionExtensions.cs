using System.Text.Json.Serialization;
using TaskTracker.Infrustructure.Extensions;

namespace TaskTracker.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public const string MyCorsPolicy = "AllowedCors";

    public static IServiceCollection ConfigureServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerSupport();
        //services.AddHttpClient();
        services.AddInfrastructureSupport();

        // Register the CORS policies
        services.AddCors(options =>
        {
            options.AddPolicy(name: MyCorsPolicy,
                builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
        });
        return services;
    }
}