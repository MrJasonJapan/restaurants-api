using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;

public class GetDishByIdQueryHandler(ILogger<GetDishByIdQueryHandler> logger, IMapper mapper, IRestaurantsRepository restaurantsRepository)
        : IRequestHandler<GetDishByIdForRestaurantQuery, DishDto?>
{
    public async Task<DishDto> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting dish {DishId} for restaurant {RestaurantId}", request.DishId, request.RestaurantId);

        var restaurant = await restaurantsRepository.GetById(request.RestaurantId) ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId) ?? throw new NotFoundException(nameof(Dish), request.DishId.ToString());

        var dishDto = mapper.Map<DishDto>(dish);

        return dishDto;
    }
}