using ECommerceAIMockUp.Application.Contracts.Authentication;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.Services.Interfaces.Authentication;
using ECommerceAIMockUp.Application.Services.Interfaces.Cart_Service;
using ECommerceAIMockUp.Application.Settings;
using ECommerceAIMockUp.Domain;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using ECommerceAIMockUp.Infrastructure.Repositories;
using ECommerceAIMockUp.Infrastructure.Services;
using ECommerceAIMockUp.Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerceAIMockUp.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            // logger service and email service


            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ApplicationConnectionString"));
            });


            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = false;

            })
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddUserManager<UserManager<AppUser>>()
                .AddDefaultTokenProviders();


            // Services of Authentication

            // to be able to authenticate users using Jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICartRepository, CartRepository>();
            return services;
        }
    }
}
