using System;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PaymentIntegration.Application.CQRS.Command;

namespace PaymentIntegration.Application
{
    public static class ApplicationRegistrationService
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddHttpClient();
            return services;
        }
    }
}
