using TaskTracker.Api.Extensions;
using TaskTracker.Infrastructure.Extensions;
using TaskTracker.Infrastructure.Persistence;

try
{
    var builder = WebApplication.CreateBuilder(args);

    //
    // builder.Host.UseSerilog((context, config) =>
    // {
    //     config.ReadFrom.Configuration(context.Configuration)
    //         .Enrich.FromLogContext()
    //         .WriteTo.Console();
    // });
    
   
    builder.Services.ConfigureServices(builder.Configuration);

    var app = builder.Build();
    
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.SeedData();
    }

    app.Configure(app.Environment);

    //Log.Information("Host starting...");
    await app.RunAsync();
   // Log.Information("Host shutting down!");
}
catch (Exception exception)
{
    Console.WriteLine(exception.Message);
    //Log.Fatal(exception, "Error starting application! '{ErrorMessage}'", exception.Message);
}
finally
{
    //Log.CloseAndFlush();
}