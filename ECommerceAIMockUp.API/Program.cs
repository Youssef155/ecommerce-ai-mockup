using ECommerceAIMockUp.API;
using ECommerceAIMockUp.Application;
using ECommerceAIMockUp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddPresentationServices()
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", "v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();