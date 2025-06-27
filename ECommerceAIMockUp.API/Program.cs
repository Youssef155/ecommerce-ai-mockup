using System.Net.Http.Headers;
using Betalgo.Ranul.OpenAI.Extensions;
using ECommerceAIMockUp.API;
using ECommerceAIMockUp.Application;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Infrastructure;
using ECommerceAIMockUp.Infrastructure.Configurations;
using ECommerceAIMockUp.Infrastructure.OpenAI;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddPresentationServices()
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddOpenAIService(settings =>
{
    settings.ApiKey = builder.Configuration["OpenAI:ApiKey"];
});

builder.Services.AddHttpClient<IImageGenerator, HuggingFaceImageGenerationService>(client =>
{
    client.BaseAddress = new Uri("https://api-inference.huggingface.co/");
    client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", builder.Configuration["HuggingFace:ApiKey"]);
});

builder.Services.Configure<DalleImageOptions>(
    builder.Configuration.GetSection("OpenAI:ImageOptions"));

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", "v1"));

    #region Add Data Seeding For Development Env
    app.Services.CreateCategories();
    app.Services.CreateProducts();
    //app.Services.CreateProductDetails();
    #endregion
}

app.UseClientCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();