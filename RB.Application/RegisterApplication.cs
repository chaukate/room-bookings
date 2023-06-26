using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RB.Application.Common.Behaviours;
using System.Reflection;

namespace RB.Application
{
    public static class RegisterApplication
    {
        public static IServiceCollection AddApplication(IServiceCollection services)
        {
            // Ref. https://code-maze.com/cqrs-mediatr-fluentvalidation/
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
