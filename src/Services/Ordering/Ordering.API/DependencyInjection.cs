using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API
{
    public static class DependencyInjection
    {
        //BEFORE BUILDING APPLICATION
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration )
        {
            // Add services to the container for the Ordering API
            // Services related to various middleware, validation, documentation, mapping, and messaging libraries
            // Carter, FluentValidation, Swagger, AutoMapper, MediatR

            services.AddCarter(); // Add Carter for routing and middleware
            services.AddExceptionHandler<CustomExceptionHandler>(); // Add custom exception handler
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database")!);

            return services;
        }


        //AFTER THE BUILDING APPLICATION
        public static WebApplication UseApiServices(this WebApplication app)
        {
            // Configure the HTTP request pipeline for the Ordering API
            // Middleware for routing, authentication, authorization, and exception handling
            // Carter, FluentValidation, Swagger, AutoMapper, MediatR

            app.MapCarter(); // Map Carter routes
            app.UseExceptionHandler(options =>
            {
            }); // Use custom exception handler
            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter  = UIResponseWriter.WriteHealthCheckUIResponse
                }); // Health checks endpoint

            return app;
        }


    }
}
