using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        // (ServiceCollectionExtensions).Assembly is the assembly project (Restaurants.Application in this case) that we want to scan.
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        // This scans all classes that inherit from IRequest and IRequestHandler and registers them in the DI container.
        // Being in the DI container means that we can inject them into other classes.
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

        // This scans all classes that inherit from Profile and add them to the AutoMapper configuration.
        // Being in the AutoMapper configuration means that we can use them to map objects from one type to another.
        // This setting also allows us to inject IMapper into classes.
        services.AddAutoMapper(applicationAssembly);

        // This scans all classes that inherit from AbstractValidator and registers them in the DI container.
        // So from the controller, we can inject the validator and use it to validate the request, 
        // and we know what requests to validate because we have scanned the assembly and know what classes have validators.
        services.AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();
    }
}