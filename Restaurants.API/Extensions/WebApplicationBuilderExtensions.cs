using Microsoft.OpenApi.Models;
using Serilog;

namespace restaurants_api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();

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

        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)
        );

    }
}