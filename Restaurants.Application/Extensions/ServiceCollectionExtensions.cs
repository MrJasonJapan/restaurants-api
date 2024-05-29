using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantsService, RestaurantsService>();

        // (ServiceCollectionExtensions).Assembly is the assembly project (Restaurants.Application in this case) that we want to scan.
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        // This scans all classes that inherit from Profile and add them to the AutoMapper configuration
        services.AddAutoMapper(applicationAssembly);

        services.AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();
    }
}