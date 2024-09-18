using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Domains.Constants;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
    IUserContext userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();

        logger.LogInformation("Authorizing user {UserEmail} for operation {Operation} on restaurant {RestaurantName}",        
            user.Email, resourceOperation, restaurant.Name);

        if(resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
        {
            logger.LogInformation("Create/Read operation - successful authorization");
            return true;
        }

        // delete
        if(resourceOperation == ResourceOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("Admin User, Delete operation - successful authorization");
            return true;
        }

        // delete or update
        if((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update)
            && user.Id == restaurant.OwnerId)
        {
            logger.LogInformation("Restaurant Owner, Delete/Update operation - successful authorization");
            return true;
        }

        return false;
    }
}