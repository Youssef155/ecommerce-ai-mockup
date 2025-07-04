using ECommerceAIMockUp.API;
using ECommerceAIMockUp.Application;
using ECommerceAIMockUp.Application.Services.Implementations;
using ECommerceAIMockUp.Application.Services.Interfaces;
using ECommerceAIMockUp.Infrastructure;
using ECommerceAIMockUp.Infrastructure.Configurations;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(option =>
    {
        option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddPresentationServices()
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);

builder.AddCutomLoggin();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", "v1"));

    #region Add Data Seeding For Development Env
    app.Services.CreateCategories();
    app.Services.CreateProducts();
    app.Services.CreateProductDetails();
    #endregion
}

app.UseClientCors();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();