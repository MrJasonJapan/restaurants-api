using Restaurants.Domain.Entities;
using Restaurants.Domains.Constants;

namespace Restaurants.Domain.Interfaces;

public interface IRestaurantAuthorizationService
{
    bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation);
}