using System.Net.Http.Headers;
using Betalgo.Ranul.OpenAI.Extensions;
using ECommerceAIMockUp.API;
using ECommerceAIMockUp.Application;
using ECommerceAIMockUp.Application.Cases.DesignCases;
using ECommerceAIMockUp.Application.Contracts.ImageGenerators;
using ECommerceAIMockUp.Application.Contracts.Repositories;
using ECommerceAIMockUp.Application.Services.Implementations;
using ECommerceAIMockUp.Application.Services.Interfaces;
using ECommerceAIMockUp.Infrastructure;
using ECommerceAIMockUp.Infrastructure.Configurations;
using ECommerceAIMockUp.Infrastructure.Repositories;
using ECommerceAIMockUp.Infrastructure.Services.ImageGeneration;
using ECommerceAIMockUp.Infrastructure.Services.ImageGeneration.OpenAI;
using ECommerceAIMockUp.Infrastructure.Services.ImageGeneration.StabiliytAIServices;
using Microsoft.Extensions.DependencyInjection;
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

builder.Services.AddOpenAIService(settings =>
{
    settings.ApiKey = builder.Configuration["OpenAI:ApiKey"];
});

builder.Services.Configure<DalleImageOptions>(
    builder.Configuration.GetSection("OpenAI:ImageOptions"));

builder.Services.AddHttpClient<IImageGenerator, StabilityAIImageGenerationService>(client =>
{
    client.BaseAddress = new Uri("https://api.stability.ai/v2beta/stable-image/generate/ultra");
    client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", builder.Configuration["StabilityAI:ApiKey"]);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/*"));
});

#region Add Generate Design Infrastructure Serives DI
builder.Services.AddScoped<IPromptValidator, PromptValidator>();
builder.Services.AddScoped<IImageStorageService, ImageStorageService>();
#endregion

#region Add Repositories DI
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IDesignRepository, DesignRepository>();
#endregion

#region Add DesignCases DI
builder.Services.AddScoped<GenerateImageCase>();
builder.Services.AddScoped<SaveImageCase>();
builder.Services.AddScoped<GetDesignsCase>();
builder.Services.AddScoped<AddDesignDetailsCase>();
#endregion



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
    await app.Services.SeedAppUserAsync();
    //app.Services.CreateProductDetails();
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