using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Domains.Constants;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger, IMapper mapper,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with id: {RestaurantId} with {@UpdatedRestaurant}", request.Id, request);

        // todo: shouldn't the logic between here and delete be in a transaction, incase someone else trys to update it or something?
        var restaurant = await restaurantsRepository.GetById(request.Id);

        if (restaurant == null)
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException();

        mapper.Map(request, restaurant);

        await restaurantsRepository.SaveChanges();
    }
}