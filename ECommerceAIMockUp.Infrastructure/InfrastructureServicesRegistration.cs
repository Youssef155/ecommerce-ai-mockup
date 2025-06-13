using ECommerceAIMockUp.Application.Contracts.Authentication;
using ECommerceAIMockUp.Application.Settings;
using ECommerceAIMockUp.Infrastructure.Contracts.Authentication;
using ECommerceAIMockUp.Infrastructure.DatabaseContext;
using ECommerceAIMockUp.Infrastructure.Identity.Models;
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


            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = false;

                //opt.SignIn.RequireConfirmedEmail = true;
            })
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<SignInManager<User>>()
                .AddUserManager<UserManager<User>>()
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
                        ValidateIssuer = true,
                        ValidateAudience = false,
                    };
                });

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            return services;
        }
    }
}
