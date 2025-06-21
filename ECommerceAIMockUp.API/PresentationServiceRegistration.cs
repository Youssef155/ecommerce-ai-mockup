using Microsoft.AspNetCore.Mvc;

namespace ECommerceAIMockUp.API
{
    public static class PresentationServiceRegistration
    {
        private const string CorsPolicyName = "Client App";
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {

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

            return services;
        }

        public static IApplicationBuilder UseClientCors(this IApplicationBuilder app)
        {
            return app.UseCors(CorsPolicyName);
        }
    }
}
