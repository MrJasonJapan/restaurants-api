using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantsService, RestaurantsService>();

        // This scan all classes that inherit from Profile and add them to the AutoMapper configuration
        // (ServiceCollectionExtensions).Assembly is the assemply (project -> Restaurants.Application) that we want to get the profiles from
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }
}