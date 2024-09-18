using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Domains.Constants;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public class DeleteDishesForRestaurantCommandHandler(ILogger<DeleteDishesForRestaurantCommandHandler> logger,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository)
    : IRequestHandler<DeleteDishesForRestaurantCommand>
{
    public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Deleting all dishes for restaurant with id: {RestaurantId}", request.RestaurantId);

        var restaurant = await restaurantsRepository.GetById(request.RestaurantId) ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbidException();

        await dishesRepository.Delete(restaurant.Dishes);
    }
}