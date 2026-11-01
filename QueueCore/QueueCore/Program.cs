using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using QueueCore.Application.Interfaces;
using QueueCore.Application.Models.Dto;
using QueueCore.Application.Services;
using QueueCore.Infrastructure.Clients;
using QueueCore.Infrastructure.Common;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING")))
{
    builder.Services.AddOpenTelemetry()
        .UseFunctionsWorkerDefaults()
        .UseAzureMonitorExporter();
}

// Http default options
builder.Services.AddHttpClient<IApiConsumer, ApiConsumer>(client =>
{
    client.DefaultRequestHeaders.Add("ApiKey", Environment.GetEnvironmentVariable("ApiKey"));
});

// configurations
builder.Services.Configure<ClientDataDto>(
    builder.Configuration.GetSection("ClientData")
    );

builder.Services.AddSingleton<ILoggerFile, LoggerFile>();
builder.Services.AddScoped<IPetitionGenerator, PetitionGeneration>();

builder.Build().Run();
