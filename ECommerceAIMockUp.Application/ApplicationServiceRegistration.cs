using ECommerceAIMockUp.Application.Services.Implementations;
using ECommerceAIMockUp.Application.Services.Interfaces.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECommerceAIMockUp.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
