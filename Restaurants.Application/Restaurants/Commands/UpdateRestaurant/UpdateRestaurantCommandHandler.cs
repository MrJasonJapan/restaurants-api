using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Commands.UpdateRestaurant;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger, IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with id: {RestaurantId} with {@UpdatedRestaurant}", request.Id, request);

        // todo: shouldn't the logic between here and delete be in a transaction, incase someone else trys to update it or something?
        var restaurant = await restaurantsRepository.GetById(request.Id);

        if (restaurant == null)
            return false;

        mapper.Map(request, restaurant);

        await restaurantsRepository.SaveChanges();

        return true;
    }
}