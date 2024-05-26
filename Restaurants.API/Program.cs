using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Create a new scope to resolve the seeder service. 
// We want to use a "scope" her because before running the application, we want to seed the database with some data as a one-time operation.
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();
await seeder.Seed();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
