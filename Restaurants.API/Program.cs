using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Application.Extensions;
using Serilog;
using restaurants_api.Middleware;
using Restaurants.Domain.Entities;
using Microsoft.OpenApi.Models;

// --- Create new app, configure services, then build it ---

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    // Allow swagger to show a UI element to save a token as specified by the user.
    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    // Add setting that will map the "bearerAuth" security definition to all endpoints, 
    // so that swagger will know to include the UI-saved token in each request.
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth"}
            },
            []
        }
    });
});

builder.Services.AddEndpointsApiExplorer(); // Required for Swagger UI to include "minimal" enpoints as added by identity framework.

builder.Services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();


// --- Seed the database as a one-time operation, with services exposed by the application ---

// Create a new scope to resolve the seeder service. 
// We want to use a "scope" her because before running the application, we want to seed the database with some data as a one-time operation.
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.Seed();


// --- Configure the HTTP request pipeline (also known as middleware), and run the app ---

// Use as the first middleware in the pipeline, so that it can catch any exceptions thrown by the application at any point in the pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseMiddleware<RequestTimeLoggingMiddleware>();

// Capture log details about executed requests
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("api/identity").MapIdentityApi<User>(); // Adds endpoints for user management (register, login, etc.), under the "api/identity" route.

app.UseAuthorization();

app.MapControllers();

app.Run();
