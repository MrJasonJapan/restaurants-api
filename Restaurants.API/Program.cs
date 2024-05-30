using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Application.Extensions;
using Serilog;
using Serilog.Events;


// --- Create new app, configure services, then build it ---

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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

// Capture log details about executed requests
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
