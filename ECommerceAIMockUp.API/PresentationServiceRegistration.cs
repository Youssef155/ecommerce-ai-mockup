using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ECommerceAIMockUp.API
{
    public static class PresentationServiceRegistration
    {
        private const string CorsPolicyName = "Client App";
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            const string serverName = "ECommerceAiMockUP";
            const string serviceVersion = "1.0.0";


            services.AddCors(opt =>
            {
                opt.AddPolicy(CorsPolicyName, builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                                .Where(x => x.Value.Errors.Count > 0)
                                .SelectMany(x => x.Value.Errors)
                                .Select(x => x.ErrorMessage).ToArray();

                    var toReturn = new { Error = errors };

                    return new BadRequestObjectResult(toReturn);
                };
            });


            services.AddOpenTelemetry().ConfigureResource(resource => resource.AddService(serverName, serviceVersion))
                .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSqlClientInstrumentation()
                .AddConsoleExporter()
                .AddOtlpExporter()
                .AddHoneycomb(c =>
                {
                    c.ServiceName = serverName;
                    c.ServiceVersion = serviceVersion;
                    c.ApiKey = "LxVPVr94wyGHc7p7dwwfVD";
                    c.Dataset = "ECommerceAI";
                }));


            return services;
        }

        public static IApplicationBuilder UseClientCors(this IApplicationBuilder app)
        {
            return app.UseCors(CorsPolicyName);
        }

        public static WebApplicationBuilder AddCutomLoggin(this WebApplicationBuilder builder)
        {
            builder.Logging
                .ClearProviders()
                .AddConsole()
                .AddDebug()
                .AddOpenTelemetry(opt =>
                {
                    opt.AddConsoleExporter()
                        .SetResourceBuilder(
                        resourceBuilder: ResourceBuilder.CreateDefault().AddService("Logging.NET BY Mostafa"))
                        .IncludeScopes = true;
                });

            return builder;
        }
    }
}
