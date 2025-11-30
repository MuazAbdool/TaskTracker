using System.Reflection;
using Microsoft.OpenApi.Models;

namespace TaskTracker.Api.Extensions;

    public static class SwaggerExtensions
    {
        public static void AddSwaggerSupport(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                // Basic API info
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TaskTracker.Api",
                    Description = "Web API for  task tracking"
                });

                opt.CustomSchemaIds(x => x.FullName);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    opt.IncludeXmlComments(xmlPath);
                }
            });
        }

    
        public static void UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(swag =>
            {
                swag.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskTracker.Api");
            });
        }
}