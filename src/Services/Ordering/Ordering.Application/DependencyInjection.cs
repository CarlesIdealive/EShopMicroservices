﻿using BuildingBlocks.Behaviours;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ordering.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add application services here - Internal concerns (CQRS, Mediator, Validators,...)
            services
                .AddMediatR(config =>
                {
                    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                    config.AddOpenBehavior(typeof(LoggingBehaviour<,>));

                });

            return services;
        }

    }



}
