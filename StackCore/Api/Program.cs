using Api.Configuration;
using Api.Middleware;
using Application.Cqrs;
using Domain.Contracts;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Application.Behaviors;
using Infrastructure.Services;
using Application.Interfaces;
using Application.Models.Dto;

var builder = WebApplication.CreateBuilder(args);
//Se agreg cnfiguracion de seguridad
builder.Services.AddOptions<SecurityConfiguration>()
    .BindConfiguration(SecurityConfiguration.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

// BD CONNECTION
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("Infrastructure") //Indica la ubicacion de las migraciones
));

// Registro de repositorios
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); //Registro de repo generico

//Registro de servicios de infraestructura
builder.Services.AddScoped<IAzureQueueService, AzureQueueService>();

// Validaciones
// ejem. builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidation>();

// Add services to the container.
builder.Services.AddControllers();


// Agreg mediator a el proceso
builder.Services.AddAppServices();
// Pipeline de validaciones
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// ENV CONFIGURATION
builder.Services.Configure<AzureConfigurationDto>(
    builder.Configuration.GetSection("AzureQueue")    
);


builder.Services.AddEndpointsApiExplorer();

// CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//Swagger config
builder.Services.AddSwaggerGen(static opc =>
{
    opc.SwaggerDoc("v1", new OpenApiInfo { Title = "UsersCore API", Version = "v1" });

    // apikey security

    var scheme = new OpenApiSecurityScheme
    {
        Name = "ApiKey",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Ingresa tu ApiKey"
    };

    // Lo agregamos a Swagger
    opc.AddSecurityDefinition("ApiKey", scheme);

    var req = new OpenApiSecurityRequirement();
    var reference = new OpenApiSecuritySchemeReference("ApiKey");
    req.Add(reference, new List<string>());
    opc.AddSecurityRequirement(doc => req);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
